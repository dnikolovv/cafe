using Cafe.Core.AuthContext;
using Cafe.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cafe.Api.Configuration
{
    public static class AuthConfiguration
    {
        public static async Task AddDefaultAdminAccountIfNoneExisting(this IApplicationBuilder app, UserManager<User> userManager, IConfiguration configuration)
        {
            var adminSection = configuration.GetSection("DefaultAdminAccount");

            var adminEmail = adminSection["Email"];
            var adminPassword = adminSection["Password"];

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
            }
        }

        private static async Task<bool> AccountExists(string email, UserManager<User> userManager) =>
            (await userManager.FindByEmailAsync(email)) != null;
    }
}
