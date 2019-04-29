using Microsoft.AspNetCore.SignalR.Client;
using Moq;
using System;
using System.Threading.Tasks;

namespace Cafe.Tests
{
    internal class TestHubConnectionFactory
    {
        public static async Task<TestHubConnection<TEvent>> OpenTestConnectionAsync<TEvent>(
            string url,
            string expectedEventToReceive,
            string accessToken = "")
        {
            var connection = await StartConnectionAsync(url, accessToken);
            var handler = new Mock<Action<TEvent>>();
            connection.On(expectedEventToReceive, handler.Object);
            return new TestHubConnection<TEvent>(connection, handler);
        }

        private static async Task<HubConnection> StartConnectionAsync(string url, string accessToken)
        {
            try
            {
                var hubConnection = new HubConnectionBuilder()
                            .WithUrl(url, o =>
                            {
                                o.AccessTokenProvider = () => Task.FromResult(accessToken);
                            })
                            .Build();

                await hubConnection.StartAsync();

                return hubConnection;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
