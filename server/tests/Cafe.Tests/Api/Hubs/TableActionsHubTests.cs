using Cafe.Core.AuthContext.Commands;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain.Events;
using Cafe.Tests.Business.AuthContext;
using Cafe.Tests.Business.TabContext.Helpers;
using Cafe.Tests.Customizations;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Hubs
{
    public class TableActionsHubTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly TabTestsHelper _tabHelper;
        private readonly AuthTestsHelper _authHelper;
        private readonly string _hubUrl;

        public TableActionsHubTests()
        {
            _fixture = new SliceFixture();
            _tabHelper = new TabTestsHelper(_fixture);
            _authHelper = new AuthTestsHelper(_fixture);
            _hubUrl = _fixture.GetFullServerUrl("/tableActions");
        }

        [Theory]
        [CustomizedAutoData]
        public async Task AssignedWaitersShouldReceiveWaiterCalledMessage(HireWaiter hireWaiterCommand, AddTable addTableCommand, Register registerCommand)
        {
            // Arrange
            var testConnection = await BuildTestTableActionConnection<WaiterCalled>(
                hireWaiterCommand,
                addTableCommand,
                registerCommand);

            await testConnection.OpenAsync();

            var callWaiterCommand = new CallWaiter
            {
                TableNumber = addTableCommand.Number
            };

            // Act
            await _fixture.SendAsync(callWaiterCommand);

            // Assert
            testConnection.VerifyMessageReceived(
                e => e.TableNumber == addTableCommand.Number &&
                     e.WaiterId == hireWaiterCommand.Id,
                Times.Once());
        }

        [Theory]
        [CustomizedAutoData]
        public async Task AssignedWaitersShouldReceiveBillRequestedMessage(HireWaiter hireWaiterCommand, AddTable addTableCommand, Register registerCommand)
        {
            // Arrange
            var testConnection = await BuildTestTableActionConnection<BillRequested>(
                hireWaiterCommand,
                addTableCommand,
                registerCommand);

            await testConnection.OpenAsync();

            var requestBillCommand = new RequestBill
            {
                TableNumber = addTableCommand.Number
            };

            // Act
            await _fixture.SendAsync(requestBillCommand);

            // Assert
            testConnection.VerifyMessageReceived(
                e => e.TableNumber == addTableCommand.Number &&
                     e.WaiterId == hireWaiterCommand.Id,
                Times.Once());
        }

        private async Task<TestHubConnection<TEvent>> BuildTestTableActionConnection<TEvent>(HireWaiter hireWaiterCommand, AddTable addTableCommand, Register registerCommand)
        {
            await _fixture.SendAsync(addTableCommand);
            await _fixture.SendAsync(hireWaiterCommand);

            var assignTableCommand = new AssignTable
            {
                WaiterId = hireWaiterCommand.Id,
                TableNumber = addTableCommand.Number
            };

            await _fixture.SendAsync(assignTableCommand);

            await _fixture.SendAsync(registerCommand);

            var assignWaiterToAccountCommand = new AssignWaiterToAccount
            {
                AccountId = registerCommand.Id,
                WaiterId = hireWaiterCommand.Id
            };

            await _fixture.SendAsync(assignWaiterToAccountCommand);

            var accessToken = (await _authHelper
                .Login(registerCommand.Email, registerCommand.Password))
                .TokenString;

            return BuildTestConnection<TEvent>(accessToken);
        }

        private TestHubConnection<TEvent> BuildTestConnection<TEvent>(string accessToken) =>
            new TestHubConnectionBuilder<TEvent>()
                .WithHub(_hubUrl)
                .WithExpectedMessage(typeof(TEvent).Name)
                .WithAccessToken(accessToken)
                .Build();
    }
}
