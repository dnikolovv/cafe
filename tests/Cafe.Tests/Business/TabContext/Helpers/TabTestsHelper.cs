using Cafe.Core.TabContext.Queries;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Tests.Business.TabContext.Helpers
{
    public class TabTestsHelper
    {
        private readonly SliceFixture _fixture;

        public TabTestsHelper(SliceFixture fixture)
        {
            _fixture = fixture;
        }

        public async Task SetupWaiterWithTable(HireWaiter hireWaiterCommand, AddTable addTableCommand)
        {
            await _fixture.SendAsync(addTableCommand);
            await _fixture.SendAsync(hireWaiterCommand);

            var assignTableCommand = new AssignTable
            {
                TableNumber = addTableCommand.Number,
                WaiterToAssignToId = hireWaiterCommand.Id
            };

            await _fixture.SendAsync(assignTableCommand);
        }

        public async Task AddMenuItems(IList<MenuItem> items)
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
    }
}
