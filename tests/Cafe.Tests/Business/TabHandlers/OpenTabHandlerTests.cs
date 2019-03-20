using Cafe.Core.TabContext.Commands;
using Cafe.Core.TabContext.Queries;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Views;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Shouldly;
using System;
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
            await SetupWaiterWithTable(hireWaiterCommand, addTableCommand);

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

        [Theory]
        [CustomizedAutoData]
        public async Task CannotOpenTheSameTabTwice(OpenTab openTabCommand, HireWaiter hireWaiterCommand, AddTable addTableCommand)
        {
            // Arrange
            await SetupWaiterWithTable(hireWaiterCommand, addTableCommand);

            // Make sure we're trying to open a tab on the added table
            openTabCommand.TableNumber = addTableCommand.Number;

            // Open a tab once
            await _fixture.SendAsync(openTabCommand);

            // Act
            var result = await _fixture.SendAsync(openTabCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Conflict);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotOpenATabOnATakenTable(OpenTab openTabCommand, HireWaiter hireWaiterCommand, AddTable addTableCommand)
        {
            // Arrange
            await SetupWaiterWithTable(hireWaiterCommand, addTableCommand);

            // Make sure we're trying to open a tab on the added table
            openTabCommand.TableNumber = addTableCommand.Number;

            // Open a tab once
            await _fixture.SendAsync(openTabCommand);

            // Act
            // Setting a new id for the command so we don't get a conflict on the tab id
            openTabCommand.Id = Guid.NewGuid();

            var result = await _fixture.SendAsync(openTabCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Conflict);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotOpenATabOnAnUnexistingTable(OpenTab openTabCommand)
        {
            // Arrange
            // We aren't setting up anything, so regardless of what tableNumber we try
            // the table should not exist

            // Act
            var result = await _fixture.SendAsync(openTabCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        private async Task SetupWaiterWithTable(HireWaiter hireWaiterCommand, AddTable addTableCommand)
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

        private async Task AssertTabExists(Guid tabId, Func<TabView, bool> predicate)
        {
            var tab = await _fixture.SendAsync(new GetTabView { Id = tabId });
            tab.Exists(predicate).ShouldBeTrue();
        }
    }
}
