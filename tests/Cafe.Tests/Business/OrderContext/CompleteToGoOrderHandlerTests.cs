using Cafe.Core.OrderContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Tests.Business.OrderContext.Helpers;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Optional;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.OrderContext
{
    public class CompleteToGoOrderHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly ToGoOrderTestsHelper _helper;

        public CompleteToGoOrderHandlerTests()
        {
            _fixture = new SliceFixture();
            _helper = new ToGoOrderTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanCompleteToGoOrder(Guid orderId, MenuItem[] items)
        {
            // Arrange
            await _helper.CreateConfirmedOrder(orderId, items);

            var commandToTest = new CompleteToGoOrder
            {
                OrderId = orderId,
                BaristaId = Option.None<Guid>()
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            await _helper.AssertOrderExists(
                orderId,
                order => order.Status == ToGoOrderStatus.Completed);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CompletedOrderIsAssignedToTheCompletingBarista(Guid orderId, Barista barista, MenuItem[] items)
        {
            // Arrange
            await _helper.AddBarista(barista);
            await _helper.CreateConfirmedOrder(orderId, items);

            var commandToTest = new CompleteToGoOrder
            {
                OrderId = orderId,
                BaristaId = barista.Id.Some<Guid>()
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            var updatedBarista = await _fixture.ExecuteDbContextAsync(dbContext =>
                dbContext
                    .Baristas
                    .Include(b => b.CompletedOrders)
                    .FirstOrDefaultAsync(b => b.Id == barista.Id));

            updatedBarista.CompletedOrders.ShouldContain(o => o.Id == orderId);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotCompleteAnOrderWithAnInvalidBaristaId(Guid orderId, MenuItem[] items)
        {
            // Arrange
            // Purposefully not adding a barista
            await _helper.CreateConfirmedOrder(orderId, items);

            var commandToTest = new CompleteToGoOrder
            {
                OrderId = orderId,
                BaristaId = Guid.NewGuid().Some() // It will not exist
            };

            // Act, Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _fixture.SendAsync(commandToTest));
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotCompleteUnconfirmedOrder(Guid orderId, MenuItem[] items)
        {
            // Arrange
            await _helper.OrderToGo(orderId, items);

            // Purposefully not confirming the order
            var commandToTest = new CompleteToGoOrder
            {
                OrderId = orderId,
                BaristaId = Option.None<Guid>()
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Validation);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotCompleteUnexistingOrder(Guid orderId)
        {
            // Arrange

            // Purposefully not creating any orders
            var commandToTest = new CompleteToGoOrder
            {
                OrderId = orderId,
                BaristaId = Option.None<Guid>()
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotCompleteAnOrderTwice(Guid orderId, MenuItem[] items)
        {
            // Arrange
            await _helper.CreateConfirmedOrder(orderId, items);

            var confirmOrderCommand = new CompleteToGoOrder
            {
                OrderId = orderId,
                BaristaId = Option.None<Guid>()
            };

            await _fixture.SendAsync(confirmOrderCommand);

            var commandToTest = new CompleteToGoOrder
            {
                OrderId = confirmOrderCommand.OrderId, // Notice the id is the same
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Validation);
        }
    }
}
