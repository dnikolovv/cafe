using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.WaiterContext
{
    public class HireWaiterHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public HireWaiterHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanHireWaiter(HireWaiter command)
        {
            // Arrange
            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.HasValue.ShouldBeTrue();

            var waiterInDb = await _fixture.ExecuteDbContextAsync(dbContext =>
                dbContext
                    .Waiters
                    .FirstOrDefaultAsync(w => w.Id == command.Id));

            waiterInDb.Id.ShouldBe(command.Id);
            waiterInDb.ShortName.ShouldBe(command.ShortName);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotHireWaiterWithATakenId(HireWaiter command)
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
