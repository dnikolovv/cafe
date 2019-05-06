using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Cafe.Tests
{
    public class TestHubConnectionBuilder<TEvent>
    {
        private string _accessToken = string.Empty;
        private string _eventName;
        private string _hubUrl;

        public TestHubConnection<TEvent> Build()
        {
            if (string.IsNullOrEmpty(_hubUrl))
                throw new InvalidOperationException($"Use {nameof(WithHub)} to set the hub url.");

            if (string.IsNullOrEmpty(_eventName))
                throw new InvalidOperationException($"Use {nameof(WithExpectedMessage)} to set the expected event name.");

            var connection = GetConnectionWithAccessToken(_hubUrl, _accessToken);

            return new TestHubConnection<TEvent>(connection, _eventName);
        }

        public TestHubConnectionBuilder<TEvent> WithHub(string hubUrl) =>
            new TestHubConnectionBuilder<TEvent>
            {
                _hubUrl = hubUrl,
                _eventName = _eventName
            };

        public TestHubConnectionBuilder<TEvent> WithAccessToken(string accessToken) =>
            new TestHubConnectionBuilder<TEvent>
            {
                _hubUrl = _hubUrl,
                _eventName = _eventName,
                _accessToken = accessToken
            };

        public TestHubConnectionBuilder<TEvent> WithExpectedMessage(string eventName) =>
            new TestHubConnectionBuilder<TEvent>
            {
                _hubUrl = _hubUrl,
                _eventName = eventName
            };

        private static HubConnection GetConnectionWithAccessToken(string url, string accessToken)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl(url, o =>
                {
                    o.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .Build();

            return hubConnection;
        }
    }
}