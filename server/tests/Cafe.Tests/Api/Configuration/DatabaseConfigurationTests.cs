using Cafe.Api.Configuration;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Configuration
{
    public class DatabaseConfigurationTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;

        public DatabaseConfigurationTests()
        {
            _fixture = new AppFixture();
        }

        [Fact]
        public Task ShouldSeedDatabase() =>
            _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                // Act
                DatabaseConfiguration.SeedDatabase(dbContext);

                // Assert
                dbContext.Waiters.Any().ShouldBeTrue();
                dbContext.Cashiers.Any().ShouldBeTrue();
                dbContext.MenuItems.Any().ShouldBeTrue();
                dbContext.Baristas.Any().ShouldBeTrue();
            });
    }
}
