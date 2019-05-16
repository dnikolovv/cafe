using AutoFixture;
using Cafe.Api.Hateoas;
using Cafe.Api.Hateoas.Resources.Tab;
using Cafe.Core.TabContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Tests.Business.TabContext.Helpers;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        LinkNames.Tab.GetTab,
                        LinkNames.Tab.Close,
                        LinkNames.Tab.OrderItems
                    };

                    await response.ShouldBeAResource<OpenTabResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task GetAllOpenTabsShouldReturnProperHypermediaLinks(Fixture fixture) =>
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

                    await _fixture.SendAsync(openTabCommand);

                    // Act
                    var response = await httpClient.GetAsync(TabRoute());

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self
                    };

                    var resource = await response.ShouldBeAResource<TabsContainerResource>(expectedLinks);

                    // Assure that the nested resource links have been properly set
                    resource.Items.ShouldAllBe(r => r.Links.Count > 0);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task OrderItemsShouldReturnProperHypermediaLinks(Guid tabId, MenuItem[] menuItems, Fixture fixture) =>
            _apiHelper.InTheContextOfAWaiter(
                waiter => async httpClient =>
                {
                    // Arrange
                    await _tabHelper.AddMenuItems(menuItems);

                    var tableNumber = waiter.ServedTables[0].Number;
                    await _tabHelper.OpenTabOnTable(tabId, tableNumber);

                    var orderItemsCommand = new OrderMenuItems
                    {
                        TabId = tabId,
                        ItemNumbers = menuItems.Select(i => i.Number).ToList()
                    };

                    // Act
                    var response = await httpClient
                        .PutAsJsonAsync(TabRoute("order"), orderItemsCommand);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Tab.GetTab,
                        LinkNames.Tab.ServeItems,
                        LinkNames.Tab.RejectItems,
                        LinkNames.Tab.Close
                    };

                    await response.ShouldBeAResource<OrderMenuItemsResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task ServeItemsShouldReturnProperHypermediaLinks(Guid tabId, MenuItem[] menuItems, Fixture fixture) =>
            _apiHelper.InTheContextOfAWaiter(
                waiter => async httpClient =>
                {
                    // Arrange
                    await _tabHelper.AddMenuItems(menuItems);

                    var tableNumber = waiter.ServedTables[0].Number;

                    await _tabHelper.OpenTabOnTable(tabId, tableNumber);
                    await _tabHelper.OrderMenuItems(tabId, menuItems);

                    var serveItemsCommand = new ServeMenuItems
                    {
                        TabId = tabId,
                        ItemNumbers = menuItems.Select(i => i.Number).ToList()
                    };

                    // Act
                    var response = await httpClient
                        .PutAsJsonAsync(TabRoute("serve"), serveItemsCommand);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Tab.GetTab,
                        LinkNames.Tab.OrderItems,
                        LinkNames.Tab.Close
                    };

                    await response.ShouldBeAResource<ServeMenuItemsResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task RejectItemsShouldReturnProperHypermediaLinks(Guid tabId, MenuItem[] menuItems, Fixture fixture) =>
            _apiHelper.InTheContextOfAWaiter(
                waiter => async httpClient =>
                {
                    // Arrange
                    await _tabHelper.AddMenuItems(menuItems);

                    var tableNumber = waiter.ServedTables[0].Number;

                    await _tabHelper.OpenTabOnTable(tabId, tableNumber);
                    await _tabHelper.OrderMenuItems(tabId, menuItems);

                    var rejectItemsCommand = new RejectMenuItems
                    {
                        TabId = tabId,
                        ItemNumbers = menuItems.Select(i => i.Number).ToList()
                    };

                    // Act
                    var response = await httpClient
                        .PutAsJsonAsync(TabRoute("reject"), rejectItemsCommand);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Tab.GetTab,
                        LinkNames.Tab.OrderItems,
                        LinkNames.Tab.Close
                    };

                    await response.ShouldBeAResource<RejectMenuItemsResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task CloseTabShouldReturnProperHypermediaLinks(Guid tabId, MenuItem[] menuItems, Fixture fixture) =>
            _apiHelper.InTheContextOfAWaiter(
                waiter => async httpClient =>
                {
                    // Arrange
                    await _tabHelper.AddMenuItems(menuItems);

                    var tableNumber = waiter.ServedTables[0].Number;

                    await _tabHelper.OpenTabOnTable(tabId, tableNumber);
                    await _tabHelper.OrderMenuItems(tabId, menuItems);

                    var closeTabCommand = new CloseTab
                    {
                        TabId = tabId,
                        AmountPaid = menuItems.Sum(i => i.Price)
                    };

                    // Act
                    var response = await httpClient
                        .PutAsJsonAsync(TabRoute("close"), closeTabCommand);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Tab.GetTab,
                        LinkNames.Tab.Open
                    };

                    await response.ShouldBeAResource<CloseTabResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task GetTabShouldReturnProperHypermediaLinksWhenTabHasOutstandingItems(Guid tabId, MenuItem[] menuItems, Fixture fixture) =>
            _apiHelper.InTheContextOfAWaiter(
                waiter => async httpClient =>
                {
                    // Arrange
                    await _tabHelper.AddMenuItems(menuItems);

                    var tableNumber = waiter.ServedTables[0].Number;

                    await _tabHelper.OpenTabOnTable(tabId, tableNumber);
                    await _tabHelper.OrderMenuItems(tabId, menuItems);

                    // Act
                    var response = await httpClient
                        .GetAsync(TabRoute(tabId.ToString()));

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Tab.OrderItems,
                        LinkNames.Tab.ServeItems,
                        LinkNames.Tab.RejectItems,
                        LinkNames.Tab.Close
                    };

                    await response.ShouldBeAResource<TabResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task GetTabShouldReturnProperHypermediaLinksWhenTabIsClosed(Guid tabId, Fixture fixture) =>
            _apiHelper.InTheContextOfAWaiter(
                waiter => async httpClient =>
                {
                    // Arrange
                    var tableNumber = waiter.ServedTables[0].Number;

                    await _tabHelper.OpenTabOnTable(tabId, tableNumber);
                    await _tabHelper.CloseTab(tabId, 0);

                    // Act
                    var response = await httpClient
                        .GetAsync(TabRoute(tabId.ToString()));

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Tab.GetTab
                    };

                    await response.ShouldBeAResource<TabResource>(expectedLinks);
                },
                fixture);

        private static string TabRoute(string route = null) =>
            $"tab/{route?.TrimStart('/') ?? string.Empty}";
    }
}
