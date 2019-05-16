using Cafe.Core.TabContext.Commands;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Tests.Business.TabContext.Helpers;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.TabContext
{
    public class CloseTabHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly TabTestsHelper _helper;

        public CloseTabHandlerTests()
        {
            _fixture = new AppFixture();
            _helper = new TabTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanCloseTabWithoutTip(Guid tabId, int tableNumber)
        {
            // Arrange
            await _helper.OpenTabOnTable(tabId, tableNumber);

            var closeTabCommand = new CloseTab
            {
                TabId = tabId,
                AmountPaid = 0 // This should be OK as we haven't ordered anything
            };

            // Act
            var result = await _fixture.SendAsync(closeTabCommand);

            // Assert
            await _helper.AssertTabExists(
                closeTabCommand.TabId,
                t => t.IsOpen == false &&
                     t.TipValue == 0);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanCloseTabWithTip(Guid tabId, int tableNumber, MenuItem[] menuItemsToOrder)
        {
            // Arrange
            await _helper.AddMenuItems(menuItemsToOrder);
            await _helper.OpenTabOnTable(tabId, tableNumber);
            await _helper.ServeMenuItems(tabId, menuItemsToOrder);

            var expectedTipAmount = 100;

            var closeTabCommand = new CloseTab
            {
                TabId = tabId,
                AmountPaid = menuItemsToOrder.Sum(i => i.Price) + expectedTipAmount
            };

            // Act
            var result = await _fixture.SendAsync(closeTabCommand);

            // Assert
            await _helper.AssertTabExists(
                closeTabCommand.TabId,
                t => t.IsOpen == false &&
                     t.TipValue == expectedTipAmount);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotCloseATabTwice(Guid tabId, int tableNumber)
        {
            // Arrange
            await _helper.OpenTabOnTable(tabId, tableNumber);

            var closeTabCommand = new CloseTab
            {
                TabId = tabId,
                AmountPaid = 0 // This should be OK as we haven't ordered anything
            };

            await _fixture.SendAsync(closeTabCommand);

            // Act
            var result = await _fixture.SendAsync(closeTabCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Validation);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotPayLessThanOwed(Guid tabId, int tableNumber, MenuItem[] menuItemsToOrder)
        {
            // Arrange
            await _helper.AddMenuItems(menuItemsToOrder);
            await _helper.OpenTabOnTable(tabId, tableNumber);
            await _helper.OrderMenuItems(tabId, menuItemsToOrder);
            await _helper.ServeMenuItems(tabId, menuItemsToOrder);

            var closeTabCommand = new CloseTab
            {
                TabId = tabId,
                AmountPaid = menuItemsToOrder.Sum(i => i.Price) - 1 // Will always be less than the price of the items
            };

            // Act
            var result = await _fixture.SendAsync(closeTabCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Validation);
        }
    }
}
