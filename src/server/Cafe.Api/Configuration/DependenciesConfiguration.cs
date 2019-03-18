using Cafe.Api.OperationFilters;
using Cafe.Core.Auth.Configuration;
using Cafe.Domain.Entities;
using Cafe.Persistance.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cafe.Api.Configuration
{
    public static class DependenciesConfiguration
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            services
                .AddDbContext<ApplicationDbContext>(opts => opts.UseSqlServer(connectionString))
                .AddEntityFrameworkSqlServer();
        }

        public static void AddJwtIdentity(this IServiceCollection services, IConfigurationSection jwtConfiguration)
        {
            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            var signingKey = new SymmetricSecurityKey(
                Encoding.Default.GetBytes(jwtConfiguration["Secret"]));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfiguration[nameof(JwtConfiguration.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtConfiguration[nameof(JwtConfiguration.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.Configure<JwtConfiguration>(options =>
            {
                options.Issuer = jwtConfiguration[nameof(JwtConfiguration.Issuer)];
                options.Audience = jwtConfiguration[nameof(JwtConfiguration.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtConfiguration[nameof(JwtConfiguration.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new Info { Title = "Cafe.Api", Version = "v1" });
                setup.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Cafe.Api.Documentation.xml"));

                setup.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Enter 'Bearer {token}' (don't forget to add 'bearer') into the field below.", Name = "Authorization", Type = "apiKey" });

                setup.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", Enumerable.Empty<string>() },
                });

                setup.OperationFilter<OptionOperationFilter>();
            });
        }
    }
}
