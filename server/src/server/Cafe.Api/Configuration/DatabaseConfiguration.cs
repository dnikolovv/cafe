using Cafe.Core.AuthContext;
using Cafe.Domain.Entities;
using Marten;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Optional;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cafe.Api.Configuration
{
    public static class DatabaseConfiguration
    {
        public static async Task<Option<(string Email, string Password)>> AddDefaultAdminAccountIfNoneExisting(UserManager<User> userManager, IConfiguration configuration)
        {
            var (adminEmail, adminPassword) = configuration.GetAdminCredentials();

            if (!await AccountExists(adminEmail, userManager))
            {
                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = adminEmail,
                    UserName = adminEmail,
                    FirstName = "Café",
                    LastName = "Admin"
                };

                await userManager.CreateAsync(adminUser, adminPassword);

                var isAdminClaim = new Claim(AuthConstants.ClaimTypes.IsAdmin, true.ToString());

                await userManager.AddClaimAsync(adminUser, isAdminClaim);

                return (adminEmail, adminPassword).Some();
            }

            return default;
        }

        public static void EnsureEventStoreIsCreated(IConfiguration configuration)
        {
            DocumentStore.For(options =>
            {
                options.Connection(configuration.GetSection("EventStore")["ConnectionString"]);
                options.CreateDatabasesForTenants(c =>
                {
                    c.ForTenant()
                        .CheckAgainstPgDatabase()
                        .WithOwner("postgres")
                        .WithEncoding("UTF-8")
                        .ConnectionLimit(-1);
                });
            });
        }

        private static async Task<bool> AccountExists(string email, UserManager<User> userManager)
        {
            var user = await userManager.FindByEmailAsync(email);

            return user != null;
        }
    }
}