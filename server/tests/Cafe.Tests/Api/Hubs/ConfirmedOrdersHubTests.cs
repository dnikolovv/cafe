using Cafe.Core.AuthContext.Commands;
using Cafe.Core.BaristaContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Tests.Business.AuthContext;
using Cafe.Tests.Business.OrderContext.Helpers;
using Cafe.Tests.Customizations;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Hubs
{
    public class ConfirmedOrdersHubTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly ToGoOrderTestsHelper _toGoOrdersHelper;
        private readonly AuthTestsHelper _authTestsHelper;
        private readonly string _hubUrl;

        public ConfirmedOrdersHubTests()
        {
            _fixture = new SliceFixture();
            _toGoOrdersHelper = new ToGoOrderTestsHelper(_fixture);
            _authTestsHelper = new AuthTestsHelper(_fixture);
            _hubUrl = _fixture.GetFullServerUrl("/confirmedOrders");
        }

        [Theory]
        [CustomizedAutoData]
        public async Task ConfirmedOrdersAreSentToAllAuthenticatedSubscribers(Guid orderId, MenuItem[] menuItems)
        {
            // Arrange
            var accessToken = await _authTestsHelper.GetAdminToken();

            var testConnection = await TestHubConnectionFactory
                .OpenTestConnectionAsync<OrderConfirmed>(_hubUrl, nameof(OrderConfirmed), accessToken);

            // Act
            await _toGoOrdersHelper.CreateConfirmedOrder(orderId, menuItems);

            // Assert
            await testConnection
                .VerifyMessageReceived(e =>
                    e.Order.Id == orderId &&
                    e.Order.OrderedItems.Count == menuItems.Length);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotConnectIfNotABarista(Register registerCommand)
        {
            // Arrange
            // The newly registered user is not going to have any roles assigned
            var accessToken = (await _authTestsHelper.RegisterAndLogin(registerCommand)).TokenString;

            var exception = await Should.ThrowAsync<HttpRequestException>(async () =>
            {
                await TestHubConnectionFactory.OpenTestConnectionAsync<OrderConfirmed>(_hubUrl, nameof(OrderConfirmed), accessToken);
            });

            exception.Message.ShouldContain("403");
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanConnectAsBarista(Register registerCommand, HireBarista hireBaristaCommand, Guid orderId, MenuItem[] menuItems)
        {
            // Arrange
            await _authTestsHelper.Register(registerCommand);
            await _fixture.SendAsync(hireBaristaCommand);

            var assignBaristaToAccountCommand = new AssignBaristaToAccount
            {
                AccountId = registerCommand.Id,
                BaristaId = hireBaristaCommand.Id
            };

            await _fixture.SendAsync(assignBaristaToAccountCommand);

            var accessToken = (await _authTestsHelper.Login(registerCommand.Email, registerCommand.Password)).TokenString;

            var testConnection = await TestHubConnectionFactory
                .OpenTestConnectionAsync<OrderConfirmed>(_hubUrl, nameof(OrderConfirmed), accessToken);

            // Act
            await _toGoOrdersHelper.CreateConfirmedOrder(orderId, menuItems);

            // Assert
            await testConnection
                .VerifyMessageReceived(e =>
                    e.Order.Id == orderId &&
                    e.Order.OrderedItems.Count == menuItems.Length);

        }

        [Fact]
        public async Task CannotConnectWithAnInvalidToken()
        {
            // Arrange
            var accessToken = "an obviously invalid access token";

            // Act, Assert
            var exception = await Should.ThrowAsync<HttpRequestException>(async () =>
            {
                await TestHubConnectionFactory.OpenTestConnectionAsync<OrderConfirmed>(_hubUrl, nameof(OrderConfirmed), accessToken);
            });

            exception.Message.ShouldContain("401");
        }
    }
}
