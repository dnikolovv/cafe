using Cafe.Tests.Extensions;
using Microsoft.AspNetCore.SignalR.Client;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cafe.Tests
{
    public class TestHubConnection<TEvent>
    {
        private readonly HubConnection _connection;
        private readonly string _expectedEventToReceive;
        private readonly Mock<Action<TEvent>> _handlerMock;

        internal TestHubConnection(HubConnection connection, string expectedEventToReceive)
        {
            if (connection.State == HubConnectionState.Connected)
            {
                throw new ArgumentException($"You shouldn't pass open connections. Use {nameof(OpenAsync)}" +
                    $"to open the connection before verifying that the message was received.");
            }

            _handlerMock = new Mock<Action<TEvent>>();
            _expectedEventToReceive = expectedEventToReceive;
            _connection = connection;
        }

        public async Task OpenAsync()
        {
            await _connection.StartAsync();
            _connection.On(_expectedEventToReceive, _handlerMock.Object);
        }

        public void VerifyMessageReceived(Expression<Func<TEvent, bool>> predicate, Times times)
        {
            _handlerMock.VerifyWithTimeout(x => x(It.Is<TEvent>(predicate)), times, 10000);
        }
    }
}