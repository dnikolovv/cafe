using Cafe.Core.AuthContext;
using Cafe.Domain.Entities;
using Cafe.Persistance.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cafe.Api.Configuration
{
    public static class AuthConfiguration
    {
        // TODO: These should come from appsettings.json
        private const string AdminEmail = "admin@cafe.org";
        private const string AdminUsername = "admin@cafe.org";
        private const string AdminPassword = "Password123$";

        public static async Task AddDefaultAdminAccountIfNoneExisting(this IApplicationBuilder app, UserManager<User> userManager)
        {
            if (!await AdminAccountExists(userManager))
            {
                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = AdminEmail,
                    UserName = AdminUsername,
                    FirstName = "Café",
                    LastName = "Admin"
                };

                await userManager.CreateAsync(adminUser, AdminPassword);

                var isAdminClaim = new Claim(AuthConstants.ClaimTypes.IsAdmin, true.ToString());

                await userManager.AddClaimAsync(adminUser, isAdminClaim);
            }
        }

        private static async Task<bool> AdminAccountExists(UserManager<User> userManager) =>
            (await userManager.FindByEmailAsync(AdminEmail)) != null;
    }
}
