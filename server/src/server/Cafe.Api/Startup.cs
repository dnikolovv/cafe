using AutoMapper;
using Cafe.Api.Configuration;
using Cafe.Api.Filters;
using Cafe.Api.Hubs;
using Cafe.Api.ModelBinders;
using Cafe.Api.Resources.Mappings;
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

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
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

            services.AddMarten(Configuration);
            services.AddCqrs();
            services.AddMediatR();
            services.AddSignalR();

            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new OptionModelBinderProvider());
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<ModelStateFilter>();
            })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<RegisterValidator>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, UserManager<User> userManager, ApplicationDbContext dbContext)
        {
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }
            else if (env.IsDevelopment())
            {
                app.UseCors(builder => builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());

                DatabaseConfiguration.RevertDatabaseToInitialState(dbContext);
                DatabaseConfiguration.AddDefaultAdminAccountIfNoneExisting(userManager, Configuration).Wait();
            }

            loggerFactory.AddLogging(Configuration.GetSection("Logging"));

            app.UseHttpsRedirection();
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
