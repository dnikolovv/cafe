using Cafe.Core.TabContext.Commands;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Tests.Business.TabContext.Helpers;
using Cafe.Tests.Customizations;
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
            MenuItem[] itemsToOrder,
            OpenTab openTabCommand,
            HireWaiter hireWaiterCommand,
            AddTable addTableCommand)
        {
            // Arrange
            await _helper.SetupWaiterWithTable(hireWaiterCommand, addTableCommand);
            await _helper.AddMenuItems(itemsToOrder);

            openTabCommand.TableNumber = addTableCommand.Number;

            await _fixture.SendAsync(openTabCommand);

            var orderItemsCommand = new OrderMenuItems
            {
                TabId = openTabCommand.Id,
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
    }
}
