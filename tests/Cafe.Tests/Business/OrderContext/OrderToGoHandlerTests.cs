using Cafe.Core.OrderContext.Commands;
using Cafe.Core.OrderContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.OrderContext
{
    public class OrderToGoHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public OrderToGoHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanOrderToGo(MenuItem[] menuItems)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.MenuItems.AddRange(menuItems);
                await dbContext.SaveChangesAsync();
            });

            var menuItemNumbers = menuItems
                .Select(i => i.Number)
                .ToArray();

            var commandToTest = new OrderToGo
            {
                Id = Guid.NewGuid(),
                ItemNumbers = menuItemNumbers
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            await AssertOrderExists(
                commandToTest.Id,
                order => order.Status == ToGoOrderStatus.Unconfirmed &&
                         order.OrderedItems.All(i => menuItemNumbers.Contains(i.Number)));
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotOrderUnexistingMenuItemsToGo(OrderToGo commandToTest)
        {
            // Arrange
            // Purposefully not adding any menu items

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotSubmitToGoOrderWithConflictingId(MenuItem[] menuItems)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.MenuItems.AddRange(menuItems);
                await dbContext.SaveChangesAsync();
            });

            var menuItemNumbers = menuItems
                .Select(i => i.Number)
                .ToArray();

            var commandToTest = new OrderToGo
            {
                Id = Guid.NewGuid(),
                ItemNumbers = menuItemNumbers
            };

            // Note that we're sending the command before the Act phase
            await _fixture.SendAsync(commandToTest);

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Conflict);
        }

        private async Task AssertOrderExists(Guid orderId, Func<ToGoOrderView, bool> predicate)
        {
            var orderView = await _fixture.SendAsync(new GetToGoOrder { Id = orderId });
            orderView.Exists(predicate).ShouldBeTrue();
        }
    }
}
