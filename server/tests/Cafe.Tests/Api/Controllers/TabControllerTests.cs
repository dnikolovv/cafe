using AutoFixture;
using Cafe.Api.Hateoas;
using Cafe.Api.Hateoas.Resources.Tab;
using Cafe.Core.TabContext.Commands;
using Cafe.Tests.Business.TabContext.Helpers;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class TabControllerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly ApiTestsHelper _apiHelper;
        private readonly TabTestsHelper _tabHelper;

        public TabControllerTests()
        {
            _fixture = new AppFixture();
            _apiHelper = new ApiTestsHelper(_fixture);
            _tabHelper = new TabTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public Task OpenTabShouldReturnProperHypermediaLinks(Fixture fixture) =>
            _apiHelper.InTheContextOfAWaiter(
                waiter => async httpClient =>
                {
                    // Arrange
                    var openTabCommand = new OpenTab
                    {
                        Id = Guid.NewGuid(),
                        CustomerName = "Some customer",
                        TableNumber = waiter.ServedTables[0].Number
                    };

                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync(TabRoute("open"), openTabCommand);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Tab.Close,
                        LinkNames.Tab.OrderItems
                    };

                    await response.ShouldBeAResource<OpenTabResource>(expectedLinks);
                },
                fixture);

        private static string TabRoute(string route) =>
            $"tab/{route?.TrimStart('/') ?? string.Empty}";
    }
}
