using AutoFixture;
using Cafe.Core.OrderContext.Queries;
using Cafe.Domain.Entities;
using Cafe.Tests.Customizations;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.OrderContext
{
    public class GetOrdersByStatusHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public GetOrdersByStatusHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanQueryOrdersByStatus(Fixture fixture)
        {
            // Arrange
            var statusToTest = ToGoOrderStatus.Pending;

            var ordersToInsert = fixture
                .Build<ToGoOrder>()
                .With(o => o.Status, statusToTest)
                .CreateMany()
                .ToList();

            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.ToGoOrders.AddRange(ordersToInsert);

                await dbContext.SaveChangesAsync();
            });

            var queryToTest = new GetOrdersByStatus
            {
                Status = statusToTest
            };

            // Act
            var result = await _fixture.SendAsync(queryToTest);

            // Assert
            result.Exists(orders =>
                orders.Count == ordersToInsert.Count &&
                orders.All(order => ordersToInsert.Any(insertedOrder =>
                    order.Id == insertedOrder.Id &&
                    order.OrderedItems.Count == insertedOrder.OrderedItems.Count &&
                    order.Status == insertedOrder.Status &&
                    !string.IsNullOrEmpty(order.StatusText))))
            .ShouldBeTrue();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanQueryOrdersByStatusWhenThereAreNone(Fixture fixture)
        {
            // Arrange
            var statusToTest = ToGoOrderStatus.Pending;

            var ordersToInsert = fixture
                .Build<ToGoOrder>()
                .With(o => o.Status, statusToTest)
                .CreateMany()
                .ToList();

            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.ToGoOrders.AddRange(ordersToInsert);

                await dbContext.SaveChangesAsync();
            });

            var queryToTest = new GetOrdersByStatus
            {
                // Notice we're querying for issued orders when there are only pending orders in the db
                Status = ToGoOrderStatus.Issued
            };

            // Act
            var result = await _fixture.SendAsync(queryToTest);

            // Assert
            result.Exists(orders => orders.Count == 0).ShouldBeTrue();
        }
    }
}
