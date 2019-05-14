using Cafe.Api.Hateoas;
using Cafe.Api.Hateoas.Resources.Manager;
using Cafe.Core.ManagerContext.Commands;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class ManagerControllerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly ApiTestsHelper _apiHelper;

        public ManagerControllerTests()
        {
            _fixture = new AppFixture();
            _apiHelper = new ApiTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public Task HireManagerShouldReturnProperHypermediaLinks(HireManager command) =>
            _apiHelper.InTheContextOfAnAdmin(
                async httpClient =>
                {
                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync("manager", command);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Manager.GetAll
                    };

                    await response.ShouldBeAResource<HireManagerResource>(expectedLinks);
                });

        [Theory]
        [CustomizedAutoData]
        public Task GetAllManagersShouldReturnProperHypermediaLinks(HireManager[] commands) =>
            _apiHelper.InTheContextOfAnAdmin(
                async httpClient =>
                {
                    // Arrange
                    await _fixture.SendManyAsync(commands);

                    // Act
                    var response = await httpClient
                        .GetAsync("manager");

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Manager.Hire
                    };

                    await response.ShouldBeAResource<ManagerContainerResource>(expectedLinks);
                });
    }
}
