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

        public static async Task AddDefaultAdminAccountIfNonExisting(this IApplicationBuilder app, ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            if (!await AdminAccountExists(userManager))
            {
                // Assign a manager and a waiter for the default admin account
                var manager = new Manager
                {
                    Id = Guid.NewGuid(),
                    ShortName = "Admin"
                };

                var waiter = new Waiter
                {
                    Id = Guid.NewGuid(),
                    ShortName = "Admin"
                };

                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = AdminEmail,
                    UserName = AdminUsername,
                    FirstName = "Café",
                    LastName = "Admin"
                };

                dbContext.Managers.Add(manager);
                dbContext.Waiters.Add(waiter);
                await dbContext.SaveChangesAsync();
                await userManager.CreateAsync(adminUser, AdminPassword);

                var claims = new List<Claim>
                {
                    new Claim(AuthConstants.ClaimTypes.IsAdmin, $"{true}"),
                    new Claim(AuthConstants.ClaimTypes.ManagerId, manager.Id.ToString()),
                    new Claim(AuthConstants.ClaimTypes.WaiterId, waiter.Id.ToString())
                };

                await userManager.AddClaimsAsync(adminUser, claims);
            }
        }

        private static async Task<bool> AdminAccountExists(UserManager<User> userManager) =>
            (await userManager.FindByEmailAsync(AdminEmail)) != null;
    }
}
