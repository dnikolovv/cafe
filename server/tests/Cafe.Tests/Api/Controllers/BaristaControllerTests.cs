using AutoFixture;
using Cafe.Api.Hateoas;
using Cafe.Api.Hateoas.Resources.Barista;
using Cafe.Core.BaristaContext.Commands;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class BaristaControllerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly ApiTestsHelper _apiHelper;

        public BaristaControllerTests()
        {
            _fixture = new AppFixture();
            _apiHelper = new ApiTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public Task HireBaristaShouldReturnProperHypermediaLinks(HireBarista command, Fixture fixture) =>
            _apiHelper.InTheContextOfAManager(
                manager => async httpClient =>
                {
                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync("barista", command);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Barista.GetAll
                    };

                    await response.ShouldBeAResource<HireBaristaResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task GetAllBaristasShouldReturnProperHypermediaLinks(HireBarista[] commands, Fixture fixture) =>
            _apiHelper.InTheContextOfAManager(
                manager => async httpClient =>
                {
                    // Arrange
                    await _fixture.SendManyAsync(commands);

                    // Act
                    var response = await httpClient
                        .GetAsync("barista");

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Barista.Hire
                    };

                    await response.ShouldBeAResource<BaristaContainerResource>(expectedLinks);
                },
                fixture);
    }
}
