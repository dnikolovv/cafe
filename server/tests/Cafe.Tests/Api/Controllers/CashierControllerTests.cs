using AutoFixture;
using Cafe.Api.Hateoas;
using Cafe.Api.Hateoas.Resources.Cashier;
using Cafe.Core.CashierContext.Commands;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class CashierControllerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly ApiTestsHelper _apiHelper;

        public CashierControllerTests()
        {
            _fixture = new AppFixture();
            _apiHelper = new ApiTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public Task HireCashierShouldReturnProperHypermediaLinks(HireCashier command, Fixture fixture) =>
            _apiHelper.InTheContextOfAManager(
                manager => async httpClient =>
                {
                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync("cashier", command);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Cashier.GetAll
                    };

                    await response.ShouldBeAResource<HireCashierResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task GetAllCashiersShouldReturnProperHypermediaLinks(HireCashier[] commands, Fixture fixture) =>
            _apiHelper.InTheContextOfAManager(
                manager => async httpClient =>
                {
                    // Arrange
                    await _fixture.SendManyAsync(commands);

                    // Act
                    var response = await httpClient
                        .GetAsync("cashier");

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Cashier.Hire
                    };

                    await response.ShouldBeAResource<CashierContainerResource>(expectedLinks);
                },
                fixture);
    }
}
