using Cafe.Api.Configuration;
using Cafe.Core.AuthContext.Commands;
using Cafe.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Configuration
{
    public class DatabaseConfigurationTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;

        public DatabaseConfigurationTests()
        {
            _fixture = new AppFixture();
        }

        [Fact]
        public async Task ShouldCreateDefaultAdminAccountWhenNotExisting()
        {
            // Arrange
            var configuration = await _fixture.ExecuteScopeAsync(async sp => sp.GetService<IConfiguration>());

            var expectedAdminCredentials = configuration.GetAdminCredentials();

            // Act
            await _fixture.ExecuteScopeAsync(async sp =>
            {
                var userManager = sp.GetService<UserManager<User>>();

                var result = await DatabaseConfiguration
                .AddDefaultAdminAccountIfNoneExisting(userManager, configuration);

                // Assert
                result.Exists(credentials =>
                    credentials.Email == expectedAdminCredentials.Email &&
                    credentials.Password == expectedAdminCredentials.Password)
                .ShouldBeTrue();
            });
        }

        [Fact]
        public async Task ShouldNotCreateDefaultAdminAccountWhenExisting()
        {
            // Arrange
            var configuration = await _fixture.ExecuteScopeAsync(async sp => sp.GetService<IConfiguration>());

            var expectedAdminCredentials = configuration.GetAdminCredentials();

            await _fixture.SendAsync(new Register
            {
                Id = Guid.NewGuid(),
                Email = expectedAdminCredentials.Email,
                Password = expectedAdminCredentials.Password,
                FirstName = "Some",
                LastName = "Account"
            });

            // Act
            await _fixture.ExecuteScopeAsync(async sp =>
            {
                var userManager = sp.GetService<UserManager<User>>();

                var result = await DatabaseConfiguration
                    .AddDefaultAdminAccountIfNoneExisting(userManager, configuration);

                // Assert
                result.HasValue.ShouldBeFalse();
            });
        }
    }
}
