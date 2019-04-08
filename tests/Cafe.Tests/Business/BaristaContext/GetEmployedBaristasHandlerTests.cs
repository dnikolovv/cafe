using Cafe.Core.BaristaContext.Queries;
using Cafe.Domain.Entities;
using Cafe.Tests.Customizations;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.BaristaContext
{
    public class GetEmployedBaristasHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public GetEmployedBaristasHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanGetAllEmployedBaristas(GetEmployedBaristas query, Barista[] baristasToHire)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Baristas.AddRange(baristasToHire);
                await dbContext.SaveChangesAsync();
            });

            // Act
            var result = await _fixture.SendAsync(query);

            // Assert
            result.Exists(baristas =>
                baristas.Count == baristasToHire.Length &&
                baristas.All(b => baristasToHire.Any(hiredBarista =>
                    b.Id == hiredBarista.Id &&
                    b.ShortName == hiredBarista.ShortName &&
                    b.CompletedOrders.Count == hiredBarista.CompletedOrders.Count &&
                    b.CompletedOrders.All(ov => hiredBarista.CompletedOrders.Any(o => o.Id == ov.Id)))))
            .ShouldBeTrue();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanGetAllEmployedBaristasWhenNoneHaveBeenHired(GetEmployedBaristas query)
        {
            // Arrange
            // Purposefully not hiring any baristas

            // Act
            var result = await _fixture.SendAsync(query);

            // Assert
            result.Exists(baristas => baristas.Count == 0).ShouldBeTrue();
        }
    }
}
