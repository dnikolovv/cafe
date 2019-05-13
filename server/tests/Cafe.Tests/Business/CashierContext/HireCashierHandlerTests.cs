using Cafe.Core.CashierContext.Commands;
using Cafe.Domain;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.CashierContext
{
    public class HireCashierHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;

        public HireCashierHandlerTests()
        {
            _fixture = new AppFixture();
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
