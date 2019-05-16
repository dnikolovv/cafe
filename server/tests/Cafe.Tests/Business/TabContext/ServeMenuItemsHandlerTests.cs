using Cafe.Core.TabContext.Commands;
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
    public class ServeMenuItemsHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly TabTestsHelper _helper;

        public ServeMenuItemsHandlerTests()
        {
            _fixture = new AppFixture();
            _helper = new TabTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanServeAllMenuItems(Guid tabId, int tableNumber, MenuItem[] itemsToServe)
        {
            // Arrange
            await _helper.AddMenuItems(itemsToServe);
            await _helper.OpenTabOnTable(tabId, tableNumber);
            await _helper.OrderMenuItems(tabId, itemsToServe);

            var serveMenuItemsCommand = new ServeMenuItems
            {
                TabId = tabId,
                ItemNumbers = itemsToServe.Select(i => i.Number).ToArray()
            };

            // Act
            var result = await _fixture.SendAsync(serveMenuItemsCommand);

            // Assert
            await _helper.AssertTabExists(
                tabId,
                t => t.OutstandingMenuItems.Count == 0 &&
                     itemsToServe.All(i => t.ServedMenuItems.Any(si => i.Number == si.Number)) &&
                     t.ServedItemsValue == itemsToServe.Sum(i => i.Price));
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotServeUnexistingItems(Guid tabId, int tableNumber, MenuItem[] itemsToServe)
        {
            // Arrange
            // Purposefully skipping adding the items to the db
            await _helper.OpenTabOnTable(tabId, tableNumber);
            await _helper.OrderMenuItems(tabId, itemsToServe);

            var serveMenuItemsCommand = new ServeMenuItems
            {
                TabId = tabId,
                ItemNumbers = itemsToServe.Select(i => i.Number).ToArray()
            };

            // Act
            var result = await _fixture.SendAsync(serveMenuItemsCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotServeOnAClosedTab(Guid tabId, int tableNumber, MenuItem[] itemsToServe)
        {
            // Arrange
            await _helper.AddMenuItems(itemsToServe);
            await _helper.OpenTabOnTable(tabId, tableNumber);
            await _helper.OrderMenuItems(tabId, itemsToServe);

            await _helper.CloseTab(tabId, itemsToServe.Sum(i => i.Price));

            var serveMenuItemsCommand = new ServeMenuItems
            {
                TabId = tabId,
                ItemNumbers = itemsToServe.Select(i => i.Number).ToArray()
            };

            // Act
            var result = await _fixture.SendAsync(serveMenuItemsCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Validation);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanServeOneMenuItemTwiceInARow(Guid tabId, int tableNumber, MenuItem[] menuItems)
        {
            // Arrange
            await _helper.AddMenuItems(menuItems);
            await _helper.OpenTabOnTable(tabId, tableNumber);

            var menuItemToServe = menuItems.First();

            // We intentionally order it twice in order to be able to assert that only one was served
            await _helper.OrderMenuItems(tabId, menuItemToServe);
            await _helper.OrderMenuItems(tabId, menuItemToServe);

            // Act
            var serveItemsCommand = new ServeMenuItems
            {
                TabId = tabId,
                ItemNumbers = new[] { menuItemToServe.Number }
            };

            await _fixture.SendAsync(serveItemsCommand);
            await _fixture.SendAsync(serveItemsCommand);

            // Assert
            await _helper.AssertTabExists(
                tabId,
                tab => tab.ServedMenuItems.Count == 2 &&
                       tab.ServedMenuItems.All(i => i.Number == menuItemToServe.Number));
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanServeOneMenuItem(Guid tabId, int tableNumber, MenuItem[] menuItems)
        {
            // Arrange
            await _helper.AddMenuItems(menuItems);
            await _helper.OpenTabOnTable(tabId, tableNumber);

            var menuItemToServe = menuItems.First();

            var serveItemsCommand = new ServeMenuItems
            {
                TabId = tabId,
                ItemNumbers = new[] { menuItemToServe.Number }
            };

            // Act
            await _fixture.SendAsync(serveItemsCommand);

            // Assert
            await _helper.AssertTabExists(
                tabId,
                tab => tab.ServedMenuItems.Count == 1 &&
                       tab.ServedMenuItems.First().Number == menuItemToServe.Number);
        }
    }
}
