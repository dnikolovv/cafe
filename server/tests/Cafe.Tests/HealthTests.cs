using Cafe.Tests.XUnit;
using Shouldly;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests
{
    public class HealthTests
    {
        private readonly SliceFixture _fixture;

        public HealthTests()
        {
            _fixture = new SliceFixture();
        }

        [Fact]
        [TestPriority(-100)]
        public async Task CanConnectToTestServer()
        {
            // Arrange
            var client = new HttpClient();

            // Act
            var result = await client.GetAsync(_fixture.GetFullServerUrl("api/health"));

            // Assert
            result.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
