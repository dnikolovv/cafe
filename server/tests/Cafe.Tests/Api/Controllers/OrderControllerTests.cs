using AutoFixture;
using Cafe.Api.Hateoas;
using Cafe.Api.Hateoas.Resources.Order;
using Cafe.Core.OrderContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Tests.Business.OrderContext.Helpers;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Optional;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class OrderControllerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly ApiTestsHelper _apiHelper;
        private readonly ToGoOrderTestsHelper _orderHelper;

        public OrderControllerTests()
        {
            _fixture = new AppFixture();
            _apiHelper = new ApiTestsHelper(_fixture);
            _orderHelper = new ToGoOrderTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public Task GetOrderShouldReturnProperHypermediaLinksWhenLoggedInAsANormalUser(Guid orderId, MenuItem[] menuItems, Fixture fixture) =>
            _apiHelper.InTheContextOfAnAuthenticatedUser(
                async client =>
                {
                    // Arrange
                    await _orderHelper.OrderToGo(orderId, menuItems);

                    // Act
                    var response = await client
                        .GetAsync(OrderRoute(orderId.ToString()));

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self
                    };

                    await response.ShouldBeAResource<ToGoOrderResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task GetOrderShouldReturnProperHypermediaLinksWhenLoggedInAsACashier(Guid orderId, MenuItem[] menuItems, Fixture fixture) =>
            _apiHelper.InTheContextOfACashier(
                cashier => async httpClient =>
                {
                    // Arrange
                    await _orderHelper.OrderToGo(orderId, menuItems);

                    // Act
                    var response = await httpClient
                        .GetAsync(OrderRoute(orderId.ToString()));

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Order.Confirm,
                        LinkNames.Order.New
                    };

                    await response.ShouldBeAResource<ToGoOrderResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task GetOrderShouldReturnProperHypermediaLinksWhenLoggedInAsABarista(Guid orderId, MenuItem[] menuItems, Fixture fixture) =>
            _apiHelper.InTheContextOfABarista(
                barista => async httpClient =>
                {
                    // Arrange
                    await _orderHelper.OrderToGo(orderId, menuItems);

                    // Act
                    var response = await httpClient
                        .GetAsync(OrderRoute(orderId.ToString()));

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Order.Complete
                    };

                    await response.ShouldBeAResource<ToGoOrderResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task GetAllOrdersShouldReturnProperHypermediaLinks(MenuItem[] menuItems, Fixture fixture) =>
            _apiHelper.InTheContextOfAnAuthenticatedUser(
                async httpClient =>
                {
                    // Arrange
                    const int countOfOrders = 5;

                    await _orderHelper.AddMenuItems(menuItems);

                    var orderCommands = fixture
                        .Build<OrderToGo>()
                        .With(x => x.ItemNumbers, menuItems.Select(i => i.Number).ToList())
                        .CreateMany(countOfOrders)
                        .ToArray();

                    await _fixture.SendManyAsync(orderCommands);

                    // Act
                    var response = await httpClient
                        .GetAsync(OrderRoute());

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Order.New
                    };

                    var resource = await response.ShouldBeAResource<ToGoOrderContainerResource>(expectedLinks);

                    resource.Items.ShouldAllBe(i => i.Links.Count > 0);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task OrderToGoShouldReturnProperHypermediaLinks(MenuItem[] menuItems, Fixture fixture) =>
            _apiHelper.InTheContextOfACashier(
                cashier => async httpClient =>
                {
                    // Arrange
                    await _orderHelper.AddMenuItems(menuItems);

                    var command = new OrderToGo
                    {
                        Id = Guid.NewGuid(),
                        ItemNumbers = menuItems.Select(i => i.Number).ToArray()
                    };

                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync(OrderRoute(), command);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Order.Get,
                        LinkNames.Order.GetAll,
                        LinkNames.Order.Confirm
                    };

                    await response.ShouldBeAResource<OrderToGoResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task ConfirmShouldReturnProperHypermediaLinks(Guid orderId, MenuItem[] menuItems, Fixture fixture) =>
            _apiHelper.InTheContextOfACashier(
                cashier => async httpClient =>
                {
                    // Arrange
                    await _orderHelper.OrderToGo(orderId, menuItems);

                    var command = new ConfirmToGoOrder
                    {
                        OrderId = orderId,
                        PricePaid = menuItems.Sum(i => i.Price)
                    };

                    // Act
                    var response = await httpClient
                        .PutAsJsonAsync(OrderRoute("confirm"), command);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Order.Get,
                        LinkNames.Order.GetAll
                    };

                    await response.ShouldBeAResource<ConfirmToGoOrderResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task CompleteShouldReturnProperHypermediaLinks(Guid orderId, MenuItem[] menuItems, Fixture fixture) =>
            _apiHelper.InTheContextOfABarista(
                barista => async httpClient =>
                {
                    // Arrange
                    await _orderHelper.CreateConfirmedOrder(orderId, menuItems);

                    var command = new CompleteToGoOrder
                    {
                        OrderId = orderId,
                        BaristaId = barista.Id.Some()
                    };

                    // Act
                    var response = await httpClient
                        .PutAsJsonAsync(OrderRoute("complete"), command);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Order.Get,
                        LinkNames.Order.GetAll
                    };

                    await response.ShouldBeAResource<CompleteToGoOrderResource>(expectedLinks);
                },
                fixture);

        private static string OrderRoute(string route = null) =>
            $"order/{route?.TrimStart('/') ?? string.Empty}";
    }
}
