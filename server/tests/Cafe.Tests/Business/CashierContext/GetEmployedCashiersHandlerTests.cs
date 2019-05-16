using Cafe.Core.CashierContext.Queries;
using Cafe.Domain.Entities;
using Cafe.Tests.Customizations;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.CashierContext
{
    public class GetEmployedCashiersHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;

        public GetEmployedCashiersHandlerTests()
        {
            _fixture = new AppFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanGetAllEmployedCashiers(GetEmployedCashiers query, Cashier[] cashiersToHire)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Cashiers.AddRange(cashiersToHire);
                await dbContext.SaveChangesAsync();
            });

            // Act
            var result = await _fixture.SendAsync(query);

            // Assert
            result.Exists(cashiers =>
                cashiers.Count == cashiersToHire.Length &&
                cashiers.All(c => cashiersToHire.Any(hiredCashier => c.Id == hiredCashier.Id &&
                                                                     c.ShortName == hiredCashier.ShortName)))
            .ShouldBeTrue();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanGetAllEmployedCashiersWhenNoneHaveBeenHired(GetEmployedCashiers query)
        {
            // Arrange
            // Purposefully not hiring any cashiers

            // Act
            var result = await _fixture.SendAsync(query);

            // Assert
            result.Exists(cashiers => cashiers.Count == 0).ShouldBeTrue();
        }
    }
}
