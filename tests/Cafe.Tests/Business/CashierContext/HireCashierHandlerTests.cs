using Cafe.Domain;
using Cafe.Tests.Customizations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.CashierContext
{
    public class HireCashierHandlerTests
    {
        private readonly SliceFixture _fixture;

        public HireCashierHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanHireCashier(HireCashier command)
        {
            // Arrange
            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.HasValue.ShouldBeTrue();

            var cashierInDb = await _fixture.ExecuteDbContextAsync(dbContext =>
                dbContext
                    .Cashiers
                    .FirstOrDefaultAsync(w => w.Id == command.Id));

            cashierInDb.Id.ShouldBe(command.Id);
            cashierInDb.ShortName.ShouldBe(command.ShortName);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotHireCashierWithATakenId(HireCashier command)
        {
            // Arrange
            await _fixture.SendAsync(command);

            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Conflict);
        }
    }
}
