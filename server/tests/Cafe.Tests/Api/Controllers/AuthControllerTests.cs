using AutoFixture;
using Cafe.Api.Hateoas;
using Cafe.Api.Hateoas.Resources.Auth;
using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
using Cafe.Tests.Business.AuthContext;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class AuthControllerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly ApiTestsHelper _apiHelper;
        private readonly AuthTestsHelper _authHelper;

        public AuthControllerTests()
        {
            _fixture = new AppFixture();
            _apiHelper = new ApiTestsHelper(_fixture);
            _authHelper = new AuthTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task LoginShouldSetProperHttpOnlyCookie(Register register)
        {
            // Arrange
            await _authHelper.Register(register);

            var loginCommand = new Login
            {
                Email = register.Email,
                Password = register.Password
            };

            // Act
            var response = await _fixture.ExecuteHttpClientAsync(client =>
                client.PostAsJsonAsync(AuthRoute("login"), loginCommand));

            // Assert
            var token = (await response
                .ShouldDeserializeTo<LoginResource>())
                .TokenString;

            response.Headers.ShouldContain(header =>
                header.Key == "Set-Cookie" &&
                header.Value.Any(x => x.Contains(AuthConstants.Cookies.AuthCookieName) && x.Contains(token)));
        }

        [Theory]
        [CustomizedAutoData]
        public async Task LoginShouldReturnProperHypermediaLinks(Register register)
        {
            // Arrange
            await _authHelper.Register(register);

            var loginCommand = new Login
            {
                Email = register.Email,
                Password = register.Password
            };

            // Act
            var response = await _fixture.ExecuteHttpClientAsync(client =>
                client.PostAsJsonAsync(AuthRoute("login"), loginCommand));

            // Assert
            var expectedLinks = new List<string>
            {
                LinkNames.Self,
                LinkNames.Auth.GetCurrentUser,
                LinkNames.Auth.Logout
            };

            await response.ShouldBeAResource<LoginResource>(expectedLinks);
        }

        [Theory]
        [CustomizedAutoData]
        public Task GetCurrentUserShouldReturnProperHypermediaLinks(Fixture fixture) =>
            _apiHelper.InTheContextOfAnAuthenticatedUser(
                async client =>
                {
                    // Act
                    var response = await client.GetAsync(AuthRoute());

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Auth.Logout
                    };

                    await response.ShouldBeAResource<UserResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task LogoutShouldReturnProperHypermediaLinks(Fixture fixture) =>
            _apiHelper.InTheContextOfAnAuthenticatedUser(
                async client =>
                {
                    // Act
                    var response = await client.DeleteAsync(AuthRoute("logout"));

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Auth.Login,
                        LinkNames.Auth.Register
                    };

                    await response.ShouldBeAResource<LogoutResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task LogoutShouldUnsetAuthCookie(Fixture fixture) =>
            _apiHelper.InTheContextOfAnAuthenticatedUser(
                    async client =>
                    {
                        // Act
                        var response = await client
                            .DeleteAsync(AuthRoute("logout"));

                        // Assert
                        response.Headers.ShouldContain(header =>
                            header.Key == "Set-Cookie" &&
                            header.Value.Any(v => v.Contains($"{AuthConstants.Cookies.AuthCookieName}=;")));
                    },
                    fixture);

        private static string AuthRoute(string route = null) =>
            $"/auth/{route?.TrimStart('/') ?? string.Empty}";
    }
}
