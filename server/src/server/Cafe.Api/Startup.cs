using AutoMapper;
using Cafe.Api.Configuration;
using Cafe.Api.Filters;
using Cafe.Api.Hateoas.Resources.Tab;
using Cafe.Api.Hubs;
using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
using Cafe.Core.AuthContext.Configuration;
using Cafe.Domain.Entities;
using Cafe.Persistance.EntityFramework;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Cafe.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext(Configuration.GetConnectionString("DefaultConnection"));

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfiles(typeof(MappingProfile).Assembly);
                cfg.AddProfiles(typeof(TabMappingProfile).Assembly);
            });

            services.AddSwagger();
            services.AddCommonServices();

            services.AddHateoas();

            services.AddJwtIdentity(
                Configuration.GetSection(nameof(JwtConfiguration)),
                options =>
                {
                    options.AddPolicy(AuthConstants.Policies.IsAdmin, pb => pb.RequireClaim(AuthConstants.ClaimTypes.IsAdmin));

                    options.AddPolicy(
                        AuthConstants.Policies.IsAdminOrManager,
                        pb => pb.IsAdminOr(ctx => ctx.User.HasClaim(c => c.Type == AuthConstants.ClaimTypes.ManagerId)));

                    options.AddPolicy(
                        AuthConstants.Policies.IsAdminOrWaiter,
                        pb => pb.IsAdminOr(ctx => ctx.User.HasClaim(c => c.Type == AuthConstants.ClaimTypes.WaiterId)));

                    options.AddPolicy(
                        AuthConstants.Policies.IsAdminOrCashier,
                        pb => pb.IsAdminOr(ctx => ctx.User.HasClaim(c => c.Type == AuthConstants.ClaimTypes.CashierId)));

                    options.AddPolicy(
                        AuthConstants.Policies.IsAdminOrBarista,
                        pb => pb.IsAdminOr(ctx => ctx.User.HasClaim(c => c.Type == AuthConstants.ClaimTypes.BaristaId)));
                });

            services.AddLogging(logBuilder => logBuilder.AddSerilog(dispose: true));

            services.AddTransient<DatabaseSeeder>();

            services.AddMarten(Configuration);
            services.AddCqrs();
            services.AddMediatR();
            services.AddSignalR();
            services.AddRepositories();

            services.AddMvc(options =>
            {
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<ModelStateFilter>();
            })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<RegisterValidator>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, UserManager<User> userManager, ApplicationDbContext dbContext, DatabaseSeeder seeder)
        {
            dbContext.Database.EnsureCreated();
            DatabaseConfiguration.EnsureEventStoreIsCreated(Configuration);

            if (!env.IsEnvironment(Environment.IntegrationTests))
            {
                DatabaseConfiguration.AddDefaultAdminAccountIfNoneExisting(userManager, Configuration).Wait();
                seeder.SeedDatabase().Wait();
            }

            if (!env.IsDevelopment())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCors(builder => builder
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .WithOrigins("http://localhost:3000", "https://*.devadventures.net")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            loggerFactory.AddLogging(Configuration.GetSection("Logging"));

            app.UseSwagger("Cafe");
            app.UseStaticFiles();
            app.UseAuthentication();

            // It's very important that UseAuthentication is called before UseSignalR
            app.UseSignalR(routes =>
            {
                routes.MapHub<ConfirmedOrdersHub>("/confirmedOrders");
                routes.MapHub<HiredWaitersHub>("/hiredWaiters");
                routes.MapHub<TableActionsHub>("/tableActions");
            });

            app.UseMvc();
        }
    }
}
