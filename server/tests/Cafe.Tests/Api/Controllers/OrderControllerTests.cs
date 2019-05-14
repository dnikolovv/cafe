using AutoFixture;
using Cafe.Api.Hateoas;
using Cafe.Domain.Entities;
using Cafe.Tests.Business.OrderContext.Helpers;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class OrderControllerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly ApiTestsHelper _apiHelper;
        private readonly ToGoOrderTestsHelper _orderHelper;

        [Theory]
        [CustomizedAutoData]
        public Task GetOrderShouldReturnProperHypermediaLinks(Guid orderId, MenuItem[] menuItems, Fixture fixture) =>
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
                        LinkNames.Self,
                        LinkNames.Order.GetAll
                    };

                    await response.ShouldBeAResource<ToGoOrderResource>()
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
                        LinkNames.Order.NewToGoOrder,
                        LinkNames.Order.GetAll
                    };

                    await response.ShouldBeAResource<ToGoOrderResource>()
                },
                fixture);

        private static string OrderRoute(string route = null) =>
            $"order/{route?.TrimStart('/') ?? string.Empty}";
    }
}
