using Microsoft.AspNetCore.SignalR.Client;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Tests
{
    internal class TestHubConnection<TEvent>
    {
        private readonly HubConnection _connection;
        private readonly Mock<Action<TEvent>> _handlerMock;

        internal TestHubConnection(HubConnection connection, Mock<Action<TEvent>> handlerMock)
        {
            _connection = connection;
            _handlerMock = handlerMock;
        }

        public async Task VerifyMessageReceived(Expression<Func<TEvent, bool>> predicate)
        {
            // The delay is required because the socket message may be received after the test method finishes execution
            // There may be a better way, but this is good enough for now
            await Task.Run(() => Thread.Sleep(200));

            _handlerMock.Verify(x => x(It.Is<TEvent>(predicate)));
        }
    }
}