using Cafe.Core.TabContext.Commands;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain;
using Cafe.Tests.Business.TabContext.Helpers;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.TabContext
{
    public class OpenTabHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly TabTestsHelper _helper;

        public OpenTabHandlerTests()
        {
            _fixture = new SliceFixture();
            _helper = new TabTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanOpenTab(OpenTab openTabCommand, HireWaiter hireWaiterCommand, AddTable addTableCommand)
        {
            // Arrange
            await _helper.SetupWaiterWithTable(hireWaiterCommand, addTableCommand);

            // Make sure we're trying to open a tab on the added table
            openTabCommand.TableNumber = addTableCommand.Number;

            // Act
            var result = await _fixture.SendAsync(openTabCommand);

            // Assert
            await _helper.AssertTabExists(
                openTabCommand.Id,
                t => t.IsOpen == true &&
                     t.WaiterName == hireWaiterCommand.ShortName &&
                     t.CustomerName == openTabCommand.CustomerName);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanOpenTabOnARecentlyFreedTable(Guid tabId, int tableNumber)
        {
            // Arrange
            await _helper.OpenTabOnTable(tabId, tableNumber);
            await _helper.CloseTab(tabId, 1);

            var commandToTest = new OpenTab
            {
                Id = Guid.NewGuid(),
                CustomerName = "Customer",
                TableNumber = tableNumber
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            await _helper.AssertTabExists(
                commandToTest.Id,
                t => t.IsOpen == true &&
                     t.TableNumber == tableNumber);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotOpenExistingTab(OpenTab openTabCommand, HireWaiter hireWaiterCommand, AddTable addTableCommand)
        {
            // Arrange
            await _helper.SetupWaiterWithTable(hireWaiterCommand, addTableCommand);

            // Make sure we're trying to open a tab on the added table
            openTabCommand.TableNumber = addTableCommand.Number;

            // Open a tab once
            await _fixture.SendAsync(openTabCommand);

            // Act
            // Make sure we won't be getting a conflict on the table numbers
            openTabCommand.TableNumber += 10;

            var result = await _fixture.SendAsync(openTabCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Conflict);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotOpenATabOnATakenTable(OpenTab openTabCommand, HireWaiter hireWaiterCommand, AddTable addTableCommand)
        {
            // Arrange
            await _helper.SetupWaiterWithTable(hireWaiterCommand, addTableCommand);

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
    }
}
