using AutoFixture;
using Cafe.Api.Hateoas;
using Cafe.Api.Hateoas.Resources.Auth;
using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
using Cafe.Core.BaristaContext.Commands;
using Cafe.Core.CashierContext.Commands;
using Cafe.Core.ManagerContext.Commands;
using Cafe.Core.WaiterContext.Commands;
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
        public async Task RegisterShouldReturnProperHypermediaLinks(Register command)
        {
            // Act
            var response = await _fixture.ExecuteHttpClientAsync(client =>
                client.PostAsJsonAsync(AuthRoute("register"), command));

            // Assert
            var expectedLinks = new List<string>
            {
                LinkNames.Self,
                LinkNames.Auth.Login
            };

            await response.ShouldBeAResource<RegisterResource>(expectedLinks);
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

                    // We don't expect any operations to be available on the user
                    // when logged in as normal
                    var expectedLinks = new List<string>
                    {
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

        [Theory]
        [CustomizedAutoData]
        public Task GetAllUserAccountShouldReturnProperHypermediaLinks(Register[] registerCommands) =>
            _apiHelper.InTheContextOfAnAdmin(
                async httpClient =>
                {
                    // Arrange
                    await _fixture.SendManyAsync(registerCommands);

                    // Act
                    var response = await httpClient.GetAsync(AuthRoute("users"));

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self
                    };

                    var resource = await response.ShouldBeAResource<UsersContainerResource>(expectedLinks);

                    var expectedPerUserLinks = new List<string>
                    {
                        LinkNames.Auth.AssignWaiter,
                        LinkNames.Auth.AssignManager,
                        LinkNames.Auth.AssignCashier,
                        LinkNames.Auth.AssignBarista
                    };

                    resource.Items.ShouldAllBe(i => expectedPerUserLinks.All(el => i.Links.Any(l => l.Key == el)));
                });

        [Theory]
        [CustomizedAutoData]
        public Task AssignManagerToAccountAccountShouldReturnProperHypermediaLinks(HireManager hireCommand, Register registerCommand) =>
            _apiHelper.InTheContextOfAnAdmin(
                async httpClient =>
                {
                    // Arrange
                    await _fixture.SendManyAsync(hireCommand, registerCommand);

                    var assignCommand = new AssignManagerToAccount
                    {
                        AccountId = registerCommand.Id,
                        ManagerId = hireCommand.Id
                    };

                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync(AuthRoute("assign/manager"), assignCommand);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Auth.AssignWaiter,
                        LinkNames.Auth.AssignCashier,
                        LinkNames.Auth.AssignBarista
                    };

                    await response.ShouldBeAResource<AssignManagerToAccountResource>(expectedLinks);
                });

        [Theory]
        [CustomizedAutoData]
        public Task AssignCashierToAccountAccountShouldReturnProperHypermediaLinks(HireCashier hireCommand, Register registerCommand) =>
            _apiHelper.InTheContextOfAnAdmin(
                async httpClient =>
                {
                    // Arrange
                    await _fixture.SendManyAsync(hireCommand, registerCommand);

                    var assignCommand = new AssignCashierToAccount
                    {
                        AccountId = registerCommand.Id,
                        CashierId = hireCommand.Id
                    };

                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync(AuthRoute("assign/cashier"), assignCommand);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Auth.AssignWaiter,
                        LinkNames.Auth.AssignManager,
                        LinkNames.Auth.AssignBarista
                    };

                    await response.ShouldBeAResource<AssignCashierToAccountResource>(expectedLinks);
                });

        [Theory]
        [CustomizedAutoData]
        public Task AssignBaristaToAccountAccountShouldReturnProperHypermediaLinks(HireBarista hireCommand, Register registerCommand) =>
            _apiHelper.InTheContextOfAnAdmin(
                async httpClient =>
                {
                    // Arrange
                    await _fixture.SendManyAsync(hireCommand, registerCommand);

                    var assignCommand = new AssignBaristaToAccount
                    {
                        AccountId = registerCommand.Id,
                        BaristaId = hireCommand.Id
                    };

                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync(AuthRoute("assign/barista"), assignCommand);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Auth.AssignWaiter,
                        LinkNames.Auth.AssignCashier,
                        LinkNames.Auth.AssignManager
                    };

                    await response.ShouldBeAResource<AssignBaristaToAccountResource>(expectedLinks);
                });

        [Theory]
        [CustomizedAutoData]
        public Task AssignWaiterToAccountAccountShouldReturnProperHypermediaLinks(HireWaiter hireCommand, Register registerCommand) =>
            _apiHelper.InTheContextOfAnAdmin(
                async httpClient =>
                {
                    // Arrange
                    await _fixture.SendManyAsync(hireCommand, registerCommand);

                    var assignCommand = new AssignWaiterToAccount
                    {
                        AccountId = registerCommand.Id,
                        WaiterId = hireCommand.Id
                    };

                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync(AuthRoute("assign/waiter"), assignCommand);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Auth.AssignManager,
                        LinkNames.Auth.AssignCashier,
                        LinkNames.Auth.AssignBarista
                    };

                    await response.ShouldBeAResource<AssignWaiterToAccountResource>(expectedLinks);
                });

        private static string AuthRoute(string route = null) =>
            $"/auth/{route?.TrimStart('/') ?? string.Empty}";
    }
}
