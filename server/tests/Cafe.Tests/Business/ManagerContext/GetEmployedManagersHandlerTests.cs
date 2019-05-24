using Cafe.Core.ManagerContext.Queries;
using Cafe.Domain.Entities;
using Cafe.Tests.Customizations;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.ManagerContext
{
    public class GetEmployedManagersHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;

        public GetEmployedManagersHandlerTests()
        {
            _fixture = new AppFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanQueryEmployedManagers(Manager[] managersToAdd)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Managers.AddRange(managersToAdd);
                await dbContext.SaveChangesAsync();
            });

            var queryToTest = new GetEmployedManagers();

            // Act
            var managers = await _fixture.SendAsync(queryToTest);

            // Assert
            managers.All(m => managersToAdd
                .Any(addedManager =>
                    m.Id == addedManager.Id &&
                    m.ShortName == addedManager.ShortName))
            .ShouldBeTrue();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanQueryManagersWhenThereAreNone(GetEmployedManagers query)
        {
            // Arrange
            // Purposefully not adding any managers

            // Act
            var managersResult = await _fixture.SendAsync(query);

            // Assert
            (managersResult.Count == 0).ShouldBeTrue();
        }
    }
}
