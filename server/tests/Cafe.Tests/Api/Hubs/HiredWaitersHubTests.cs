using Cafe.Core.AuthContext.Commands;
using Cafe.Core.ManagerContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain.Events;
using Cafe.Tests.Business.AuthContext;
using Cafe.Tests.Customizations;
using Moq;
using Shouldly;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Hubs
{
    public class HiredWaitersHubTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly AuthTestsHelper _authTestsHelper;
        private readonly string _hubUrl;

        public HiredWaitersHubTests()
        {
            _fixture = new SliceFixture();
            _authTestsHelper = new AuthTestsHelper(_fixture);
            _hubUrl = _fixture.GetCompleteServerUrl("/hiredWaiters");
        }

        [Theory]
        [CustomizedAutoData]
        public async Task AuthenticatedSubscribersAreNotifiedAboutHiredWaiters(HireWaiter hireWaiterCommand)
        {
            // Arrange
            var adminAccessToken = await _authTestsHelper.GetAdminToken();

            var testConnection = BuildTestConnection(adminAccessToken);

            await testConnection.OpenAsync();

            // Act
            await _fixture.SendAsync(hireWaiterCommand);

            // Assert
            testConnection
                .VerifyMessageReceived(
                    e => e.Waiter.Id == hireWaiterCommand.Id &&
                         e.Waiter.ShortName == hireWaiterCommand.ShortName,
                    Times.Once());
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotConnectIfNotAManager(Register registerCommand)
        {
            // Arrange
            // The newly registered user is not going to have any roles assigned
            var accessToken = (await _authTestsHelper.RegisterAndLogin(registerCommand)).TokenString;

            var exception = await Should.ThrowAsync<HttpRequestException>(async () =>
            {
                await BuildTestConnection(accessToken)
                    .OpenAsync();
            });

            exception.Message.ShouldContain("403");
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanConnectAsManager(Register registerCommand, HireManager hireManagerCommand, HireWaiter hireWaiterCommand)
        {
            // Arrange
            await _authTestsHelper.Register(registerCommand);
            await _fixture.SendAsync(hireManagerCommand);

            var assignManagerToAccountCommand = new AssignManagerToAccount
            {
                AccountId = registerCommand.Id,
                ManagerId = hireManagerCommand.Id
            };

            await _fixture.SendAsync(assignManagerToAccountCommand);

            var accessToken = (await _authTestsHelper.Login(registerCommand.Email, registerCommand.Password)).TokenString;

            var testConnection = BuildTestConnection(accessToken);

            await testConnection.OpenAsync();

            // Act
            await _fixture.SendAsync(hireWaiterCommand);

            // Assert
            testConnection
                .VerifyMessageReceived(
                    e => e.Waiter.Id == hireWaiterCommand.Id &&
                         e.Waiter.ShortName == hireWaiterCommand.ShortName,
                    Times.Once());

        }

        [Fact]
        public async Task CannotConnectWithAnInvalidToken()
        {
            // Arrange
            var accessToken = "an obviously invalid access token";

            // Act, Assert
            var exception = await Should.ThrowAsync<HttpRequestException>(async () =>
            {
                await BuildTestConnection(accessToken)
                    .OpenAsync();
            });

            exception.Message.ShouldContain("401");
        }

        private TestHubConnection<WaiterHired> BuildTestConnection(string accessToken) =>
            new TestHubConnectionBuilder<WaiterHired>()
                .WithHub(_hubUrl)
                .WithExpectedMessage(nameof(WaiterHired))
                .WithAccessToken(accessToken)
                .Build();
    }
}
