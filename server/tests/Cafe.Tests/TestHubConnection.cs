using Cafe.Tests.Extensions;
using Microsoft.AspNetCore.SignalR.Client;
using Moq;
using System;
using System.Linq.Expressions;

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

        public void VerifyMessageReceived(Expression<Func<TEvent, bool>> predicate, Times times)
        {
            _handlerMock.VerifyWithTimeout(x => x(It.Is<TEvent>(predicate)), times, 10000);
        }
    }
}