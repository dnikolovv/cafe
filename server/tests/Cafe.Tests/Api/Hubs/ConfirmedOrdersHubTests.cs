using Cafe.Core.AuthContext.Commands;
using Cafe.Core.OrderContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Views;
using Cafe.Tests.Business.AuthContext;
using Cafe.Tests.Business.OrderContext.Helpers;
using Cafe.Tests.Customizations;
using Microsoft.AspNetCore.SignalR.Client;
using Shouldly;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Hubs
{
    public class ConfirmedOrdersHubTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly ToGoOrderTestsHelper _helper;
        private readonly AuthTestsHelper _authTestsHelper;

        public ConfirmedOrdersHubTests()
        {
            _fixture = new SliceFixture();
            _helper = new ToGoOrderTestsHelper(_fixture);
            _authTestsHelper = new AuthTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task ConfirmedOrdersAreSentToAllAuthenticatedSubscribers(Guid orderId, MenuItem[] menuItems)
        {
            // Arrange
            // Login
            var baseUrl = SliceFixture.BaseUrl;

            var token = await GetAdminToken();

            var hubConnection = await StartConnectionAsync(baseUrl + "/confirmedOrders", token);

            OrderConfirmed testOrder = null;

            hubConnection.On<OrderConfirmed>(nameof(OrderConfirmed), order =>
            {
                testOrder = order;
            });

            // Act
            await _helper.CreateConfirmedOrder(orderId, menuItems);

            // The delay is required because the socket message may be received after the method finishes execution
            // There may be a better way, but this is good enough for now
            await Task.Run(() => Thread.Sleep(200));

            // Assert
            testOrder.ShouldNotBeNull();
        }

        private static async Task<HubConnection> StartConnectionAsync(string url, string accessToken = "")
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl(url, o =>
                {
                    o.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .Build();

            await hubConnection.StartAsync();

            return hubConnection;
        }

        private async Task<string> GetAdminToken()
        {
            var (Email, Password) = await _authTestsHelper.RegisterAdminAccount();

            var token = (await _authTestsHelper
                .Login(Email, Password))
                .TokenString;

            return token;
        }
    }
}
