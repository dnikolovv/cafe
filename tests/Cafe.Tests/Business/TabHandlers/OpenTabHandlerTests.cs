using Cafe.Core.TabContext.Commands;
using Cafe.Core.TabContext.Queries;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;
using Cafe.Tests.Customizations;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.TabHandlers
{
    public class OpenTabHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public OpenTabHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanOpenTab(OpenTab openTabCommand, HireWaiter hireWaiterCommand, AddTable addTableCommand)
        {
            // Arrange
            await _fixture.SendAsync(addTableCommand);
            await _fixture.SendAsync(hireWaiterCommand);

            var assignTableCommand = new AssignTable
            {
                TableNumber = addTableCommand.Number,
                WaiterToAssignToId = hireWaiterCommand.Id
            };

            await _fixture.SendAsync(assignTableCommand);

            // Make sure we're trying to open a tab on the added table
            openTabCommand.TableNumber = addTableCommand.Number;

            // Act
            var result = await _fixture.SendAsync(openTabCommand);

            // Assert
            await AssertTabExists(
                openTabCommand.Id,
                t => t.IsOpen == true &&
                     t.WaiterName == hireWaiterCommand.ShortName &&
                     t.CustomerName == openTabCommand.CustomerName);
        }

        private async Task AssertTabExists(Guid tabId, Func<TabView, bool> predicate)
        {
            var tab = await _fixture.SendAsync(new GetTabView { Id = tabId });
            tab.Exists(predicate).ShouldBeTrue();
        }
    }
}
