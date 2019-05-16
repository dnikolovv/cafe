using Cafe.Core.BaristaContext.Commands;
using Cafe.Domain;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.BaristaContext
{
    public class HireBaristaHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;

        public HireBaristaHandlerTests()
        {
            _fixture = new AppFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanHireBarista(HireBarista command)
        {
            // Arrange
            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.HasValue.ShouldBeTrue();

            var baristaInDb = await _fixture.ExecuteDbContextAsync(dbContext =>
                dbContext
                    .Baristas
                    .Include(b => b.CompletedOrders)
                    .FirstOrDefaultAsync(b => b.Id == command.Id &&
                                              b.ShortName == command.ShortName &&
                                              b.CompletedOrders.Count == 0));

            baristaInDb.Id.ShouldBe(command.Id);
            baristaInDb.ShortName.ShouldBe(command.ShortName);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotHireBaristaWithATakenId(HireBarista command)
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
