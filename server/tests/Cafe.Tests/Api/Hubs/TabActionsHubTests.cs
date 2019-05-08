using Cafe.Core.AuthContext.Commands;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Tests.Business.AuthContext;
using Cafe.Tests.Business.TabContext.Helpers;
using Cafe.Tests.Customizations;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Hubs
{
    public class TabActionsHubTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly TabTestsHelper _tabHelper;
        private readonly AuthTestsHelper _authHelper;
        private readonly string _hubUrl;

        public TabActionsHubTests()
        {
            _fixture = new SliceFixture();
            _tabHelper = new TabTestsHelper(_fixture);
            _authHelper = new AuthTestsHelper(_fixture);
            _hubUrl = "/tabActions";
        }

        [Theory]
        [CustomizedAutoData]
        public async Task SubscribedWaiterShouldReceiveMessagesForHisAssignedTable(HireWaiter hireWaiterCommand, AddTable addTableCommand, Register registerCommand)
        {
            // Arrange
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

            var testWaiterCalledConnection = BuildTestConnection<WaiterCalled>(accessToken);
            var testBillRequestedConnection = BuildTestConnection<BillRequested>(accessToken);

            await testWaiterCalledConnection.OpenAsync();
            await testBillRequestedConnection.OpenAsync();

            var callWaiterCommand = new CallWaiter
            {
                TableNumber = addTableCommand.Number
            };

            var requestBillCommand = new RequestBill
            {
                TableNumber = addTableCommand.Number
            };

            // Act
            await _fixture.SendAsync(callWaiterCommand);
            await _fixture.SendAsync(requestBillCommand);

            // Assert
            testWaiterCalledConnection.VerifyMessageReceived(e => e.WaiterId == hireWaiterCommand.Id, Times.Once());
            testBillRequestedConnection.VerifyMessageReceived(e => e.WaiterId == hireWaiterCommand.Id, Times.Once());
        }

        private TestHubConnection<TEvent> BuildTestConnection<TEvent>(string accessToken) =>
            new TestHubConnectionBuilder<TEvent>()
                .WithHub(_hubUrl)
                .WithExpectedMessage(nameof(TEvent))
                .WithAccessToken(accessToken)
                .Build();
    }
}
