using AutoFixture;
using Cafe.Api.Hateoas;
using Cafe.Api.Hateoas.Resources.Home;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class HomeControllerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly ApiTestsHelper _apiHelper;

        public HomeControllerTests()
        {
            _fixture = new AppFixture();
            _apiHelper = new ApiTestsHelper(_fixture);
        }

        [Fact]
        public Task RequestingTheApiRootShouldReturnProperHypermediaLinksWhenNotLoggedIn() =>
            _apiHelper.InTheContextOfAnAnonymousUser(
                async httpClient =>
                {
                    // Act
                    var response = await httpClient
                        .GetAsync(httpClient.BaseAddress);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Auth.Login,
                        LinkNames.Auth.Register
                    };

                    await response.ShouldBeAResource<ApiRootResource>(expectedLinks);
                });

        [Theory]
        [CustomizedAutoData]
        public Task RequestingTheApiRootShouldReturnProperHypermediaLinksWhenLoggedIn(Fixture fixture) =>
            _apiHelper.InTheContextOfAnAuthenticatedUser(
                async httpClient =>
                {
                    // Act
                    var response = await httpClient
                        .GetAsync(httpClient.BaseAddress);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Auth.Logout
                    };

                    await response.ShouldBeAResource<ApiRootResource>(expectedLinks);
                },
                fixture);
    }
}
