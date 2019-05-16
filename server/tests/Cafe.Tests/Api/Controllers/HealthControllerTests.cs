using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class HealthControllerTests
    {
        private readonly AppFixture _fixture;
        private readonly ApiTestsHelper _apiHelper;

        public HealthControllerTests()
        {
            _fixture = new AppFixture();
            _apiHelper = new ApiTestsHelper(_fixture);
        }

        [Fact]
        public Task CanConnectToTestServer() =>
            _apiHelper.InTheContextOfAnAnonymousUser(
                async httpClient =>
                {
                    // Act
                    var response = await httpClient
                        .GetAsync("health");

                    // Assert
                    response.StatusCode.ShouldBe(HttpStatusCode.OK);
                });
    }
}
