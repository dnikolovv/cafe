using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Tests.Business.AuthContext;
using Cafe.Tests.Business.OrderContext.Helpers;
using Cafe.Tests.Customizations;
using System;
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
            var hubUrl = _fixture.GetFullServerUrl("/confirmedOrders");
            var token = await _authTestsHelper.GetAdminToken();

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
    }
}
