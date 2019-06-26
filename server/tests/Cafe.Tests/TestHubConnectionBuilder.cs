using System;
using System.Collections.Generic;

namespace Cafe.Tests
{
    public class TestHubConnectionBuilder
    {
        private List<(Type Type, string Name)> _expectedEventNames;
        private string _hubUrl;
        private string _accessToken;

        public TestHubConnection Build()
        {
            if (string.IsNullOrEmpty(_hubUrl))
                throw new InvalidOperationException($"Use {nameof(OnHub)} to set the hub url.");

            if (_expectedEventNames == null || _expectedEventNames.Count == 0)
                throw new InvalidOperationException($"Use {nameof(WithExpectedEvent)} to set the expected event name.");

            var testConnection = new TestHubConnection(_hubUrl, _accessToken);

            foreach (var expected in _expectedEventNames)
            {
                testConnection.Expect(expected.Name, expected.Type);
            }

            Clear();

            return testConnection;
        }

        public TestHubConnectionBuilder OnHub(string hubUrl)
        {
            _hubUrl = hubUrl;
            return this;
        }

        public TestHubConnectionBuilder WithExpectedEvent<TEvent>(string eventName)
        {
            if (_expectedEventNames == null)
                _expectedEventNames = new List<(Type, string)>();

            _expectedEventNames.Add((typeof(TEvent), eventName));
            return this;
        }

        public TestHubConnectionBuilder WithAccessToken(string accessToken)
        {
            _accessToken = accessToken;
            return this;
        }

        private void Clear()
        {
            _expectedEventNames = null;
            _accessToken = null;
            _hubUrl = null;
        }
    }
}