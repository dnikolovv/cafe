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
    public class RejectMenuItemsHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly TabTestsHelper _helper;

        public RejectMenuItemsHandlerTests()
        {
            _fixture = new SliceFixture();
            _helper = new TabTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanRejectAllItems(Guid tabId, int tableNumber, MenuItem[] items)
        {
            // Arrange
            await _helper.AddMenuItems(items);
            await _helper.OpenTabOnTable(tabId, tableNumber);
            await _helper.OrderMenuItems(tabId, items);

            var rejectItemsCommand = new RejectMenuItems
            {
                TabId = tabId,
                ItemNumbers = items.Select(i => i.Number).ToList()
            };

            // Act
            var result = await _fixture.SendAsync(rejectItemsCommand);

            // Assert
            await _helper.AssertTabExists(
                tabId,
                t => t.RejectedMenuItems.Count == items.Length &&
                     t.OutstandingMenuItems.Count == 0 &&
                     t.ServedMenuItems.Count == 0 &&
                     t.RejectedItemsValue == items.Sum(i => i.Price) &&
                     items.All(i => t.RejectedMenuItems.Any(ri => ri.Number == i.Number)));
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanRejectOneItem(Guid tabId, int tableNumber, MenuItem[] items)
        {
            // Arrange
            await _helper.AddMenuItems(items);
            await _helper.OpenTabOnTable(tabId, tableNumber);
            await _helper.OrderMenuItems(tabId, items);

            var itemToReject = items.First();

            var rejectItemsCommand = new RejectMenuItems
            {
                TabId = tabId,
                ItemNumbers = new[] { itemToReject.Number }
            };

            // Act
            var result = await _fixture.SendAsync(rejectItemsCommand);

            // Assert
            await _helper.AssertTabExists(
                tabId,
                t => t.RejectedMenuItems.Count == 1 &&
                     t.RejectedItemsValue == itemToReject.Price &&
                     t.RejectedMenuItems.First().Number == itemToReject.Number &&
                     t.OutstandingMenuItems.Count == items.Length - 1);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanRejectOneItemOfMultipleOrdered(Guid tabId, int tableNumber, MenuItem item, int timesToOrder)
        {
            // Arrange
            await _helper.AddMenuItems(item);
            await _helper.OpenTabOnTable(tabId, tableNumber);

            // Note that we're ordering the same item multiple times
            var itemsToOrder = Enumerable
                .Repeat(item, timesToOrder)
                .ToArray();

            await _helper.OrderMenuItems(tabId, itemsToOrder);

            var rejectItemsCommand = new RejectMenuItems
            {
                TabId = tabId,
                ItemNumbers = new[] { item.Number }
            };

            // Act
            var result = await _fixture.SendAsync(rejectItemsCommand);

            // Assert
            await _helper.AssertTabExists(
                tabId,
                t => t.RejectedMenuItems.Count == 1 &&
                     t.RejectedItemsValue == item.Price &&
                     t.RejectedMenuItems.First().Number == item.Number &&
                     t.OutstandingMenuItems.Count == itemsToOrder.Length - 1);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotRejectUnorderedItems(Guid tabId, int tableNumber, MenuItem[] items)
        {
            // Arrange
            await _helper.AddMenuItems(items);
            await _helper.OpenTabOnTable(tabId, tableNumber);

            // Purposefully skipping the order
            var rejectItemsCommand = new RejectMenuItems
            {
                TabId = tabId,
                ItemNumbers = items.Select(i => i.Number).ToList()
            };

            // Act
            var result = await _fixture.SendAsync(rejectItemsCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Validation);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotRejectItemsOnAClosedTab(Guid tabId, int tableNumber, MenuItem[] items)
        {
            // Arrange
            await _helper.AddMenuItems(items);
            await _helper.OpenTabOnTable(tabId, tableNumber);
            await _helper.OrderMenuItems(tabId, items);
            await _helper.CloseTab(tabId, items.Sum(i => i.Price));

            var rejectItemsCommand = new RejectMenuItems
            {
                TabId = tabId,
                ItemNumbers = items.Select(i => i.Number).ToList()
            };

            // Act
            var result = await _fixture.SendAsync(rejectItemsCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Validation);
        }
    }
}
