using Cafe.Core.AuthContext.Commands;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain.Events;
using Cafe.Tests.Business.AuthContext;
using Cafe.Tests.Customizations;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Hubs
{
    // TODO: The test argument names are weird
    public class TableActionsHubTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly AuthTestsHelper _authHelper;
        private readonly string _hubUrl;

        public TableActionsHubTests()
        {
            _fixture = new AppFixture();
            _authHelper = new AuthTestsHelper(_fixture);
            _hubUrl = _fixture.GetCompleteServerUrl("/tableActions");
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

        [Theory]
        [CustomizedAutoData]
        public async Task UnassignedToWaitersAccountsShouldNotReceiveBillRequestedMessage(
            HireWaiter hireAssignedWaiterCommand,
            HireWaiter hireUnassignedWaiterCommand,
            AddTable addTableCommand,
            Register registerCommand)
        {
            // Arrange
            var testConnection = await BuildTestHubConnectionWithoutAssignedWaiter<BillRequested>(
                hireAssignedWaiterCommand,
                hireUnassignedWaiterCommand,
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
            testConnection.VerifyNoMessagesWereReceived();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task UnassignedToWaitersAccountsShouldNotReceiveWaiterCalledMessage(
            HireWaiter hireAssignedWaiterCommand,
            HireWaiter hireUnassignedWaiterCommand,
            AddTable addTableCommand,
            Register registerCommand)
        {
            // Arrange
            var testConnection = await BuildTestHubConnectionWithoutAssignedWaiter<WaiterCalled>(
                hireAssignedWaiterCommand,
                hireUnassignedWaiterCommand,
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
            testConnection.VerifyNoMessagesWereReceived();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CallWaiterMessageShouldBeCorrectlySentWhenMultipleSubscribersAreAttached(
            HireWaiter hireAssignedWaiterCommand,
            HireWaiter hireUnassignedWaiterCommand,
            AddTable addTableCommand,
            Register registerAssignedAccount,
            Register registerUnassignedAccount)
        {
            // Arrange
            var invalidTestConnection = await BuildTestHubConnectionWithoutAssignedWaiter<WaiterCalled>(
                hireAssignedWaiterCommand,
                hireUnassignedWaiterCommand,
                addTableCommand,
                registerUnassignedAccount);

            var validTestConnection = await BuildTestTableActionConnection<WaiterCalled>(
                hireAssignedWaiterCommand,
                addTableCommand,
                registerAssignedAccount);

            await invalidTestConnection.OpenAsync();
            await validTestConnection.OpenAsync();

            var callWaiterCommand = new CallWaiter
            {
                TableNumber = addTableCommand.Number
            };

            // Act
            await _fixture.SendAsync(callWaiterCommand);

            // Assert
            invalidTestConnection.VerifyNoMessagesWereReceived();

            validTestConnection.VerifyMessageReceived(
                e => e.TableNumber == addTableCommand.Number &&
                     e.WaiterId == hireAssignedWaiterCommand.Id,
                Times.Once());
        }

        [Theory]
        [CustomizedAutoData]
        public async Task BillRequestedMessageShouldBeCorrectlySentWhenMultipleSubscribersAreAttached(
            HireWaiter hireAssignedWaiterCommand,
            HireWaiter hireUnassignedWaiterCommand,
            AddTable addTableCommand,
            Register registerAssignedAccount,
            Register registerUnassignedAccount)
        {
            // Arrange
            var invalidTestConnection = await BuildTestHubConnectionWithoutAssignedWaiter<BillRequested>(
                hireAssignedWaiterCommand,
                hireUnassignedWaiterCommand,
                addTableCommand,
                registerUnassignedAccount);

            var validTestConnection = await BuildTestTableActionConnection<BillRequested>(
                hireAssignedWaiterCommand,
                addTableCommand,
                registerAssignedAccount);

            await invalidTestConnection.OpenAsync();
            await validTestConnection.OpenAsync();

            var requestBillCommand = new RequestBill
            {
                TableNumber = addTableCommand.Number
            };

            // Act
            await _fixture.SendAsync(requestBillCommand);

            // Assert
            invalidTestConnection.VerifyNoMessagesWereReceived();

            validTestConnection.VerifyMessageReceived(
                e => e.TableNumber == addTableCommand.Number &&
                     e.WaiterId == hireAssignedWaiterCommand.Id,
                Times.Once());
        }

        private async Task<TestHubConnection<TEvent>> BuildTestHubConnectionWithoutAssignedWaiter<TEvent>(
            HireWaiter hireAssignedWaiterCommand,
            HireWaiter hireUnassignedWaiterCommand,
            AddTable addTableCommand,
            Register registerCommand)
        {
            await HireWaiterWithTable(hireAssignedWaiterCommand, addTableCommand);
            await _fixture.SendAsync(hireUnassignedWaiterCommand);

            await _fixture.SendAsync(registerCommand);

            var assignWaiterToAccountCommand = new AssignWaiterToAccount
            {
                AccountId = registerCommand.Id,

                // Purposefully assigning another waiter to the account, that is not assigned to the table
                WaiterId = hireUnassignedWaiterCommand.Id
            };

            await _fixture.SendAsync(assignWaiterToAccountCommand);

            var accessToken = (await _authHelper
                .Login(registerCommand.Email, registerCommand.Password))
                .TokenString;

            var testConnection = BuildTestConnection<TEvent>(accessToken);

            return testConnection;
        }

        private async Task<TestHubConnection<TEvent>> BuildTestTableActionConnection<TEvent>(HireWaiter hireWaiterCommand, AddTable addTableCommand, Register registerCommand)
        {
            await HireWaiterWithTable(hireWaiterCommand, addTableCommand);

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

        private async Task HireWaiterWithTable(HireWaiter hireWaiterCommand, AddTable addTableCommand)
        {
            await _fixture.SendAsync(addTableCommand);
            await _fixture.SendAsync(hireWaiterCommand);

            var assignTableCommand = new AssignTable
            {
                WaiterId = hireWaiterCommand.Id,
                TableNumber = addTableCommand.Number
            };

            await _fixture.SendAsync(assignTableCommand);
        }

        private TestHubConnection<TEvent> BuildTestConnection<TEvent>(string accessToken) =>
            new TestHubConnectionBuilder<TEvent>()
                .WithHub(_hubUrl)
                .WithExpectedMessage(typeof(TEvent).Name)
                .WithAccessToken(accessToken)
                .Build();
    }
}
