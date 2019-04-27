using Cafe.Api;
using Cafe.Core.AuthContext.Commands;
using Cafe.Core.OrderContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Views;
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

        public ConfirmedOrdersHubTests()
        {
            _fixture = new SliceFixture();
            _helper = new ToGoOrderTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task ConfirmedOrdersAreSentToAllAuthenticatedSubscribers(Guid orderId, MenuItem[] menuItems)
        {
            // Arrange
            // TODO: Make sure port is not taken

            // Host the app
            const string baseUrl = "http://localhost:7777";

            var webhost = Program
                .CreateWebHostBuilder(new string[] { "--environment", "Development" }, baseUrl)
                .Build();

            webhost.Start();

            // Login
            var client = new HttpClient();

            // TODO: Get credentials from configuration
            var loginCommand = new Login
            {
                Email = "admin@cafe.org",
                Password = "Password123$"
            };

            var loginResponse = await client.PostAsJsonAsync(baseUrl + "/api/auth/login", loginCommand);

            // TODO: Handle errors
            var token = (await loginResponse.Content.ReadAsAsync<JwtView>()).TokenString;

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            // Instantiate the hub connection
            var hubConnection = await StartConnectionAsync(baseUrl + "/confirmedOrders", token);

            // Subscribe to the relevant message
            OrderConfirmed testOrder = null;

            hubConnection.On<OrderConfirmed>(nameof(OrderConfirmed), order =>
            {
                testOrder = order;
            });

            // Prepare the order to be confirmed
            await _helper.OrderToGo(orderId, menuItems);

            var confirmOrderCommand = new ConfirmToGoOrder
            {
                OrderId = orderId,
                PricePaid = menuItems.Sum(i => i.Price)
            };

            // Act
            // Confirm the order
            var result = await client.PutAsJsonAsync(baseUrl + "/api/order/confirm", confirmOrderCommand);

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
    }
}
