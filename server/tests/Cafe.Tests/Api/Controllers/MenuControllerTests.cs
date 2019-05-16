using AutoFixture;
using Cafe.Api.Hateoas;
using Cafe.Api.Hateoas.Resources.Menu;
using Cafe.Core.MenuContext.Commands;
using Cafe.Tests.Business.OrderContext.Helpers;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class MenuControllerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly ApiTestsHelper _apiHelper;
        private readonly ToGoOrderTestsHelper _orderHelper;

        public MenuControllerTests()
        {
            _fixture = new AppFixture();
            _apiHelper = new ApiTestsHelper(_fixture);
            _orderHelper = new ToGoOrderTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public Task GetMenuItemsShouldReturnProperHypermediaLinksWhenLoggedInAsANormalUser(AddMenuItems command, Fixture fixture) =>
            _apiHelper.InTheContextOfAnAuthenticatedUser(
                async client =>
                {
                    // Arrange
                    await _fixture.SendAsync(command);

                    // Act
                    var response = await client
                        .GetAsync(MenuRoute("items"));

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self
                    };

                    var resource = await response.ShouldBeAResource<MenuItemsContainerResource>(expectedLinks);

                    resource.Items.ShouldAllBe(i => i.Links.Count == 0);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task GetMenuItemsShouldReturnProperHypermediaLinksWhenLoggedInAsAManager(AddMenuItems command, Fixture fixture) =>
            _apiHelper.InTheContextOfAManager(
                manager => async client =>
                {
                    // Arrange
                    await _fixture.SendAsync(command);

                    // Act
                    var response = await client
                        .GetAsync(MenuRoute("items"));

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self
                    };

                    var resource = await response.ShouldBeAResource<MenuItemsContainerResource>(expectedLinks);

                    var expectedMenuItemLinks = new List<string>
                    {
                        LinkNames.Menu.AddMenuItem
                    };

                    resource.Items.ShouldAllBe(i => i.Links.All(l => expectedMenuItemLinks.Contains(l.Key)));
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task AddMenuItemsShouldReturnProperHypermediaLinks(AddMenuItems command, Fixture fixture) =>
            _apiHelper.InTheContextOfAManager(
                manager => async client =>
                {
                    // Act
                    var response = await client
                        .PostAsJsonAsync(MenuRoute("items"), command);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Menu.GetAllMenuItems
                    };

                    var resource = await response.ShouldBeAResource<AddMenuItemsResource>(expectedLinks);
                },
                fixture);

        private static string MenuRoute(string route = null) =>
            $"menu/{route?.TrimStart('/') ?? string.Empty}";
    }
}
