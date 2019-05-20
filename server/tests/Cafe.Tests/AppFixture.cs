using Cafe.Api;
using Cafe.Core;
using Cafe.Persistance.EntityFramework;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Cafe.Tests
{
    public class AppFixture
    {
        public static readonly string BaseUrl;
        private static readonly IConfiguration _configuration;
        private static readonly IServiceScopeFactory _scopeFactory;

        static AppFixture()
        {
            BaseUrl = $"http://localhost:{GetFreeTcpPort()}";

            var webhost = Program
                .CreateWebHostBuilder(new string[] { "--environment", "IntegrationTests" }, BaseUrl)
                .Build();

            webhost.Start();

            var scopeFactory = (IServiceScopeFactory)webhost.Services.GetService(typeof(IServiceScopeFactory));

            _scopeFactory = scopeFactory;

            using (var scope = scopeFactory.CreateScope())
            {
                _configuration = scope.ServiceProvider.GetService<IConfiguration>();
            }
        }

        public static string EventStoreConnectionString => _configuration.GetSection("EventStore")["ConnectionString"];
        public static string RelationalDbConnectionString => _configuration.GetConnectionString("DefaultConnection");

        public Task ExecuteDbContextAsync(Func<ApplicationDbContext, Task> action) =>
            ExecuteScopeAsync(sp =>
            {
                var dbContext = sp.GetService<ApplicationDbContext>();

                return action(dbContext);
            });

        public Task<T> ExecuteDbContextAsync<T>(Func<ApplicationDbContext, Task<T>> action) =>
            ExecuteScopeAsync(sp =>
            {
                var dbContext = sp.GetService<ApplicationDbContext>();

                return action(dbContext);
            });

        public Task<TResult> ExecuteHttpClientAsync<TResult>(Func<HttpClient, Task<TResult>> action, string accessToken = null)
        {
            var client = BuildHttpClient(accessToken);
            return action(client);
        }

        public Task ExecuteHttpClientAsync(Func<HttpClient, Task> action, string accessToken = null)
        {
            var client = BuildHttpClient(accessToken);
            return action(client);
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    await action(scope.ServiceProvider).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var result = await action(scope.ServiceProvider).ConfigureAwait(false);

                    return result;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public string GetCompleteServerUrl(string route)
        {
            route = route.TrimStart('/', '\\');
            return $"{BaseUrl}/{route}";
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public Task SendAsync(IRequest request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public Task SendManyAsync(params ICommand[] commands)
        {
            return ExecuteScopeAsync(async sp =>
            {
                var mediator = sp.GetService<IMediator>();

                foreach (var command in commands)
                {
                    await mediator.Send(command);
                }

                return Task.CompletedTask;
            });
        }

        private static HttpClient BuildHttpClient(string accessToken = null)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };

            if (!string.IsNullOrEmpty(accessToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            }

            return client;
        }

        private static int GetFreeTcpPort()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}