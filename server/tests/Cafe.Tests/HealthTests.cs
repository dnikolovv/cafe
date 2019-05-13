using Shouldly;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests
{
    public class HealthTests
    {
        private readonly AppFixture _fixture;

        public HealthTests()
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
