using Shouldly;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class HealthControllerTests
    {
        private readonly AppFixture _fixture;

        public HealthControllerTests()
        {
            _fixture = new AppFixture();
        }

        [Fact]
        public async Task CanConnectToTestServer()
        {
            // Arrange
            var client = new HttpClient();

            // Act
            var result = await client.GetAsync(_fixture.GetCompleteServerUrl("health"));

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
