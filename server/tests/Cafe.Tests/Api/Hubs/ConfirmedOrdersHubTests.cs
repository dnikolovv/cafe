using Cafe.Core.AuthContext.Commands;
using Cafe.Core.OrderContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Views;
using Cafe.Tests.Business.AuthContext;
using Cafe.Tests.Business.OrderContext.Helpers;
using Cafe.Tests.Customizations;
using Microsoft.AspNetCore.SignalR.Client;
using Moq;
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
            var hubUrl = SliceFixture.BaseUrl + "/confirmedOrders";
            var token = await GetAdminToken();

            var testConnection = await TestHubConnectionFactory
                .CreateTestConnectionAsync<OrderConfirmed>(hubUrl, nameof(OrderConfirmed), token);

            // Act
            await _helper.CreateConfirmedOrder(orderId, menuItems);

            // Assert
            await testConnection
                .VerifyMessageReceived(e =>
                    e.Order.Id == orderId &&
                    e.Order.OrderedItems.Count == menuItems.Length);
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
