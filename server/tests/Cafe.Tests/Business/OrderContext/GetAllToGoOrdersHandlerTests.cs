using Cafe.Core.OrderContext.Queries;
using Cafe.Domain.Entities;
using Cafe.Tests.Customizations;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.OrderContext
{
    public class GetAllToGoOrdersHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public GetAllToGoOrdersHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanQueryForOrders(ToGoOrder[] ordersToAdd)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.ToGoOrders.AddRange(ordersToAdd);
                await dbContext.SaveChangesAsync();
            });

            var queryToTest = new GetAllToGoOrders();

            // Act
            var result = await _fixture.SendAsync(queryToTest);

            // Assert
            result.Exists(ordersResult =>
                ordersResult.All(o => ordersToAdd
                    .SingleOrDefault(addedOrder =>
                        o.Id == addedOrder.Id &&
                        o.Status == addedOrder.Status &&
                        o.OrderedItems.Count == addedOrder.OrderedItems.Count &&
                        o.OrderedItems.Sum(i => i.Price) == addedOrder.OrderedItems.Sum(i => i.MenuItem.Price)) != null))
            .ShouldBeTrue();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanQueryOrdersWhenThereAreNone(GetAllToGoOrders query)
        {
            // Arrange
            // Purposefully not adding any orders

            // Act
            var result = await _fixture.SendAsync(query);

            // Assert
            result.Exists(orders => orders.Count == 0).ShouldBeTrue();
        }
    }
}
