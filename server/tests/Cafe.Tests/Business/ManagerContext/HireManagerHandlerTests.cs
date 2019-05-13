using Cafe.Core.ManagerContext.Commands;
using Cafe.Domain;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.ManagerContext
{
    public class HireManagerHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;

        public HireManagerHandlerTests()
        {
            _fixture = new AppFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanHireManager(HireManager command)
        {
            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            var managerInDb = await _fixture.ExecuteDbContextAsync(dbContext =>
            {
                var manager = dbContext
                    .Managers
                    .SingleAsync(m => m.Id == command.Id);

                return manager;
            });

            managerInDb.Id.ShouldBe(command.Id);
            managerInDb.ShortName.ShouldBe(command.ShortName);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotHireManagerWithATakenId(HireManager command)
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
