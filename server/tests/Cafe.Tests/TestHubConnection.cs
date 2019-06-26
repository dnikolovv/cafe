using Cafe.Tests.Extensions;
using Microsoft.AspNetCore.SignalR.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Cafe.Tests
{
    public class TestHubConnection
    {
        private readonly HubConnection _connection;
        private readonly Dictionary<Type, object> _handlersMap;
        private readonly int _verificationTimeout;

        internal TestHubConnection(string url, string accessToken, int verificationTimeout = 10000)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(url, opts =>
                {
                    opts.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .Build();

            _verificationTimeout = verificationTimeout;
            _handlersMap = new Dictionary<Type, object>();
        }

        public void Expect<TEvent>(string expectedName)
        {
            var handlerMock = new Mock<Action<TEvent>>();
            RegisterHandler(handlerMock);
            _connection.On(expectedName, handlerMock.Object);
        }

        public void Expect(string expectedName, Type expectedType)
        {
            var genericExpectMethod = GetGenericMethod(
                nameof(Expect),
                new[] { expectedType });

            genericExpectMethod.Invoke(this, new[] { expectedName });
        }

        public async Task OpenAsync()
        {
            await _connection.StartAsync();
        }

        public async Task VerifyMessageReceived<TEvent>(Expression<Func<TEvent, bool>> predicate, Times times)
        {
            if (!_handlersMap.ContainsKey(typeof(TEvent)))
                throw new HandlerNotRegisteredException(typeof(TEvent));

            var handlersForType = _handlersMap[typeof(TEvent)];

            foreach (var handler in (List<Mock<Action<TEvent>>>)handlersForType)
            {
                await handler.VerifyWithTimeoutAsync(
                    x => x(It.Is(predicate)),
                    times,
                    _verificationTimeout);
            }
        }

        public async Task VerifyNoMessagesWereReceived()
        {
            if (_handlersMap.Count == 0)
                return;

            foreach (var eventType in _handlersMap.Keys)
            {
                var verifyMethod = GetGenericMethod(
                    nameof(VerifyMessageReceived),
                    new[] { eventType });

                await (Task)verifyMethod.Invoke(
                    this,
                    new object[] { ItIsAnyObjectPredicate(eventType), Times.Never() });
            }
        }

        private LambdaExpression ItIsAnyObjectPredicate(Type eventType)
        {
            var parameter = Expression.Parameter(eventType, "x");
            var body = Expression.Constant(true);
            var selector = Expression.Lambda(body, parameter);
            return selector;
        }

        private MethodInfo GetGenericMethod(string name, Type[] genericArguments)
        {
            var method = typeof(TestHubConnection)
                .GetMethods()
                .First(m => m.ContainsGenericParameters && m.Name == name)
                .MakeGenericMethod(genericArguments);

            return method;
        }

        private void RegisterHandler<TEvent>(Mock<Action<TEvent>> handler)
        {
            if (!_handlersMap.TryGetValue(typeof(TEvent), out object handlersForType))
            {
                handlersForType = new List<Mock<Action<TEvent>>>();
                _handlersMap[typeof(TEvent)] = handlersForType;
            }

            var handlers = (List<Mock<Action<TEvent>>>)handlersForType;
            handlers.Add(handler);
        }
    }
}