using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Persistance.EntityFramework;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cafe.Api.Configuration
{
    public class DatabaseSeeder
    {
        private static readonly List<Barista> _baristas = new List<Barista>
        {
            new Barista
            {
                Id = Guid.NewGuid(),
                ShortName = "John"
            },
            new Barista
            {
                Id = Guid.NewGuid(),
                ShortName = "James"
            }
        };

        private static readonly List<Cashier> _cashiers = new List<Cashier>
        {
            new Cashier
            {
                Id = Guid.NewGuid(),
                ShortName = "Steve"
            }
        };

        private static readonly List<Manager> _managers = new List<Manager>
        {
            new Manager
            {
                Id = Guid.NewGuid(),
                ShortName = "Jamie"
            }
        };

        private static readonly List<MenuItem> _menuItems = new List<MenuItem>
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
            new MenuItem
            {
                Id = Guid.NewGuid(),
                Description = "Fanta",
                Number = 4,
                Price = 2
            },
            new MenuItem
            {
                Id = Guid.NewGuid(),
                Description = "Latte",
                Number = 5,
                Price = 2.5m
            }
        };

        private static readonly List<Table> _tables = new List<Table>
        {
            new Table
            {
                Id = Guid.NewGuid(),
                Number = 1
            },
            new Table
            {
                Id = Guid.NewGuid(),
                Number = 2
            },
            new Table
            {
                Id = Guid.NewGuid(),
                Number = 3
            },
            new Table
            {
                Id = Guid.NewGuid(),
                Number = 4
            }
        };

        private static readonly List<Waiter> _waiters = new List<Waiter>
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
            },
            new Waiter
            {
                Id = Guid.NewGuid(),
                ShortName = "Steve"
            }
        };

        private readonly ApplicationDbContext _dbContext;
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public DatabaseSeeder(IMediator mediator, ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            _mediator = mediator;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task SeedDatabase()
        {
            if (!DatabaseIsEmpty())
                return;

            _dbContext.AddRange(_menuItems);
            _dbContext.AddRange(_cashiers);
            _dbContext.AddRange(_baristas);
            _dbContext.AddRange(_waiters);
            _dbContext.AddRange(_tables);
            _dbContext.AddRange(_managers);

            await _dbContext.SaveChangesAsync();

            await AssignWaitersToTables(_waiters, _tables);

            await RegisterAccount("waiter@cafe.org", "Password123$", new Claim(AuthConstants.ClaimTypes.WaiterId, _waiters[0].Id.ToString()));
            await RegisterAccount("cashier@cafe.org", "Password123$", new Claim(AuthConstants.ClaimTypes.CashierId, _cashiers[0].Id.ToString()));
            await RegisterAccount("barista@cafe.org", "Password123$", new Claim(AuthConstants.ClaimTypes.BaristaId, _baristas[0].Id.ToString()));
            await RegisterAccount("manager@cafe.org", "Password123$", new Claim(AuthConstants.ClaimTypes.ManagerId, _managers[0].Id.ToString()));
        }

        private async Task AssignWaitersToTables(List<Waiter> waiters, List<Table> tables)
        {
            if (tables.Count < waiters.Count)
                throw new InvalidOperationException("You need at least as much tables as waiters.");

            for (int i = 0; i < waiters.Count; i++)
            {
                var waiter = waiters[i];
                var table = tables[i];

                var assignCommand = new AssignTable
                {
                    WaiterId = waiter.Id,
                    TableNumber = table.Number
                };

                var result = await _mediator.Send(assignCommand);
            }
        }

        private bool DatabaseIsEmpty() =>
            !_dbContext.MenuItems.Any() &&
            !_dbContext.Waiters.Any() &&
            !_dbContext.Cashiers.Any();

        private async Task RegisterAccount(string email, string password, params Claim[] claims)
        {
            var registerCommand = new Register
            {
                Id = Guid.NewGuid(),
                Email = email,
                Password = password,
                FirstName = "Test",
                LastName = "Account"
            };

            await _mediator.Send(registerCommand);

            var user = await _userManager
                .FindByEmailAsync(registerCommand.Email);

            await _userManager.AddClaimsAsync(user, claims);
        }
    }
}