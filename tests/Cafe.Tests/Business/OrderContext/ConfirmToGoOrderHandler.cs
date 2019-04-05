using Cafe.Core.OrderContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Tests.Business.OrderContext.Helpers;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.OrderContext
{
    public class ConfirmToGoOrderHandler : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly ToGoOrderTestsHelper _helper;

        public ConfirmToGoOrderHandler()
        {
            _fixture = new SliceFixture();
            _helper = new ToGoOrderTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanConfirmToGoOrder(Guid orderId, MenuItem[] items)
        {
            // Arrange
            await _helper.OrderToGo(orderId, items);

            var commandToTest = new ConfirmToGoOrder
            {
                OrderId = orderId,
                PricePaid = items.Sum(i => i.Price)
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            await _helper.AssertOrderExists(
                orderId,
                order => order.Status == ToGoOrderStatus.Confirmed);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotConfirmUnexistingOrder(Guid orderId)
        {
            // Arrange
            // Purposefully not creating any orders
            var commandToTest = new ConfirmToGoOrder
            {
                OrderId = orderId,
                PricePaid = 100
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotConfirmAnOrderTwice(Guid orderId, MenuItem[] items)
        {
            // Arrange
            await _helper.OrderToGo(orderId, items);

            var confirmOrderCommand = new ConfirmToGoOrder
            {
                OrderId = orderId,
                PricePaid = items.Sum(i => i.Price)
            };

            await _fixture.SendAsync(confirmOrderCommand);

            var commandToTest = new ConfirmToGoOrder
            {
                OrderId = confirmOrderCommand.OrderId, // Notice the id is the same
                PricePaid = items.Sum(i => i.Price)
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Validation);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotPayLessThanOwed(Guid orderId, MenuItem[] items)
        {
            // Arrange
            await _helper.OrderToGo(orderId, items);

            var commandToTest = new ConfirmToGoOrder
            {
                OrderId = orderId,
                PricePaid = items.Sum(i => i.Price) - 1 // Notice we're paying less than what the items are worth
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Validation);
        }
    }
}
