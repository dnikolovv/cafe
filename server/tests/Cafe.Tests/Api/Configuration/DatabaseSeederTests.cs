using Cafe.Api.Configuration;
using Cafe.Persistance.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Configuration
{
    public class DatabaseSeederTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;

        public DatabaseSeederTests()
        {
            _fixture = new AppFixture();
        }

        [Fact]
        public Task ShouldSeedDatabase() =>
            _fixture.ExecuteScopeAsync(async sp =>
            {
                // Arrange
                var databaseSeeder = sp.GetService<DatabaseSeeder>();
                var dbContext = sp.GetService<ApplicationDbContext>();

                // Act
                await databaseSeeder.SeedDatabase();

                // Assert
                dbContext.Waiters.Any().ShouldBeTrue();
                dbContext.Cashiers.Any().ShouldBeTrue();
                dbContext.MenuItems.Any().ShouldBeTrue();
                dbContext.Baristas.Any().ShouldBeTrue();
                dbContext.Managers.Any().ShouldBeTrue();
                dbContext.Users.Any().ShouldBeTrue();
            });
    }
}
