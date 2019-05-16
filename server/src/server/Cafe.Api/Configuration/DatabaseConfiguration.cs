using Cafe.Core.AuthContext;
using Cafe.Domain.Entities;
using Cafe.Persistance.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cafe.Api.Configuration
{
    public static class DatabaseConfiguration
    {
        public static void SeedDatabase(ApplicationDbContext dbContext)
        {
            if (dbContext.MenuItems.Any() && dbContext.Waiters.Any() && dbContext.Cashiers.Any())
                return;

            var menuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Coffee",
                    Number = 1,
                    Price = 1
                },
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Tea",
                    Number = 2,
                    Price = 1
                },
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Coke",
                    Number = 3,
                    Price = 2
                },
            };

            var waiters = new List<Waiter>
            {
                new Waiter
                {
                    Id = Guid.NewGuid(),
                    ShortName = "Pete"
                },
                new Waiter
                {
                    Id = Guid.NewGuid(),
                    ShortName = "James"
                }
            };

            var tables = new List<Table>
            {
                new Table
                {
                    Id = Guid.NewGuid(),
                    Number = 1,
                    WaiterId = waiters[0].Id
                },
                new Table
                {
                    Id = Guid.NewGuid(),
                    Number = 2,
                    WaiterId = null
                }
            };

            var baristas = new List<Barista>
            {
                new Barista
                {
                    Id = Guid.NewGuid(),
                    ShortName = "John"
                }
            };

            var cashiers = new List<Cashier>
            {
                new Cashier
                {
                    Id = Guid.NewGuid(),
                    ShortName = "Steve"
                }
            };

            dbContext.MenuItems.AddRange(menuItems);
            dbContext.Waiters.AddRange(waiters);
            dbContext.Tables.AddRange(tables);
            dbContext.Baristas.AddRange(baristas);
            dbContext.Cashiers.AddRange(cashiers);

            dbContext.SaveChanges();
        }

        public static async Task<(string Email, string Password)> AddDefaultAdminAccountIfNoneExisting(UserManager<User> userManager, IConfiguration configuration)
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

            return (adminEmail, adminPassword);
        }

        private static async Task<bool> AccountExists(string email, UserManager<User> userManager)
        {
            var user = await userManager.FindByEmailAsync(email);

            return user != null;
        }
    }
}
