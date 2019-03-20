using Cafe.Api;
using Cafe.Core;
using Cafe.Persistance.EntityFramework;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Cafe.Tests
{
    public class SliceFixture
    {
        private static readonly IConfigurationRoot _configuration;
        private static readonly IServiceScopeFactory _scopeFactory;

        static SliceFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var startup = new Startup(_configuration);
            var services = new ServiceCollection();

            startup.ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            _scopeFactory = provider.GetService<IServiceScopeFactory>();

            var dbContext = provider.GetService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        }

        public static string DbConnectionString => _configuration.GetConnectionString("DefaultConnection");

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
                    // Block left for debugging purposes
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
                    // Block left for debugging purposes
                    throw;
                }
            }
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
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                foreach (var command in commands)
                {
                    mediator.Send(command);
                }

                return Task.CompletedTask;
            });
        }
    }
}
