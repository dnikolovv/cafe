using Cafe.Core.TabContext.Commands;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Tests.Business.TabContext.Helpers;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.TabContext
{
    public class OrderMenuItemsHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly TabTestsHelper _helper;

        public OrderMenuItemsHandlerTests()
        {
            _fixture = new SliceFixture();
            _helper = new TabTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanOrderMenuItems(
            Guid tabId,
            int tableNumber,
            MenuItem[] itemsToOrder)
        {
            // Arrange
            await _helper.OpenTabOnTable(tabId, tableNumber);
            await _helper.AddMenuItems(itemsToOrder);

            var orderItemsCommand = new OrderMenuItems
            {
                TabId = tabId,
                ItemNumbers = itemsToOrder.Select(i => i.Number).ToList()
            };

            // Act
            var result = await _fixture.SendAsync(orderItemsCommand);

            // Assert
            await _helper.AssertTabExists(
                orderItemsCommand.TabId,
                t => t.IsOpen == true &&
                     t.OrderedMenuItems.Any() &&
                     orderItemsCommand.ItemNumbers.All(n => t.OrderedMenuItems.Any(i => i.Number == n)));
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotOrderUnexistingItems(
            Guid tabId,
            int tableNumber,
            int[] menuItemNumbersToOrder)
        {
            // Arrange
            await _helper.OpenTabOnTable(tabId, tableNumber);

            // Purposefully not adding any items
            var orderItemsCommand = new OrderMenuItems
            {
                TabId = tabId,
                ItemNumbers = menuItemNumbersToOrder
            };

            // Act
            var result = await _fixture.SendAsync(orderItemsCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotOrderItemsOnAnUnexistingTab(
            MenuItem[] itemsToOrder,
            HireWaiter hireWaiterCommand,
            AddTable addTableCommand)
        {
            // Arrange
            await _helper.SetupWaiterWithTable(hireWaiterCommand, addTableCommand);

            // Purposefully not opening a tab
            var orderItemsCommand = new OrderMenuItems
            {
                TabId = Guid.NewGuid(),
                ItemNumbers = itemsToOrder.Select(i => i.Number).ToList()
            };

            // Act
            var result = await _fixture.SendAsync(orderItemsCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotOrderItemsOnAClosedTab(
            Guid tabId,
            int tableNumber,
            MenuItem[] itemsToOrder)
        {
            // Arrange
            await _helper.AddMenuItems(itemsToOrder);
            await _helper.OpenTabOnTable(tabId, tableNumber);

            var closeTabCommand = new CloseTab
            {
                TabId = tabId,
                AmountPaid = 0
            };

            await _fixture.SendAsync(closeTabCommand);

            var orderItemsCommand = new OrderMenuItems
            {
                TabId = tabId,
                ItemNumbers = itemsToOrder.Select(i => i.Number).ToList()
            };

            // Act
            var result = await _fixture.SendAsync(orderItemsCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Validation);
        }
    }
}
