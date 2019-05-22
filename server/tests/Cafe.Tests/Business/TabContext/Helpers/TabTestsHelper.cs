using Cafe.Core.TabContext.Commands;
using Cafe.Core.TabContext.Queries;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;
using MediatR;
using Optional;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.Tests.Business.TabContext.Helpers
{
    public class TabTestsHelper
    {
        private readonly AppFixture _fixture;

        public TabTestsHelper(AppFixture fixture)
        {
            _fixture = fixture;
        }

        public async Task AddMenuItems(params MenuItem[] items)
        {
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.MenuItems.AddRange(items);
                await dbContext.SaveChangesAsync();
            });
        }

        public async Task AssertTabExists(Guid tabId, Func<TabView, bool> predicate)
        {
            var tab = await _fixture.SendAsync(new GetTabView { Id = tabId });
            tab.Exists(predicate).ShouldBeTrue();
        }

        public async Task OrderMenuItems(Guid tabId, params MenuItem[] items)
        {
            var itemNumbers = items.Select(i => i.Number).ToList();

            var orderItems = new OrderMenuItems
            {
                TabId = tabId,
                ItemNumbers = itemNumbers
            };

            var result = await _fixture.SendAsync(orderItems);
        }

        public async Task ServeMenuItems(Guid tabId, params MenuItem[] items)
        {
            var itemNumbers = items.Select(i => i.Number).ToList();

            var serveItems = new ServeMenuItems
            {
                TabId = tabId,
                ItemNumbers = itemNumbers
            };

            await _fixture.SendAsync(serveItems);
        }

        public async Task<Option<Unit, Error>> OpenTabOnTable(Guid tabId, int tableNumber)
        {
            await SetupWaiterWithTable(
                new HireWaiter { Id = Guid.NewGuid(), ShortName = $"Waiter{Guid.NewGuid().ToString()}" },
                new AddTable { Id = Guid.NewGuid(), Number = tableNumber });

            var openTabCommand = new OpenTab
            {
                Id = tabId,
                CustomerName = $"Customer{Guid.NewGuid().ToString()}",
                TableNumber = tableNumber
            };

            return await _fixture.SendAsync(openTabCommand);
        }

        public async Task CloseTab(Guid tabId, decimal amountPaid)
        {
            var command = new CloseTab
            {
                TabId = tabId,
                AmountPaid = amountPaid
            };

            await _fixture.SendAsync(command);
        }

        public async Task SetupWaiterWithTable(HireWaiter hireWaiterCommand, AddTable addTableCommand)
        {
            await _fixture.SendAsync(addTableCommand);
            await _fixture.SendAsync(hireWaiterCommand);

            var assignTableCommand = new AssignTable
            {
                TableNumber = addTableCommand.Number,
                WaiterId = hireWaiterCommand.Id
            };

            await _fixture.SendAsync(assignTableCommand);
        }
    }
}