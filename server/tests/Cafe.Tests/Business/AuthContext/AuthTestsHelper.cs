using Cafe.Api.Configuration;
using Cafe.Core.AuthContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shouldly;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cafe.Tests.Business.AuthContext
{
    public class AuthTestsHelper
    {
        private readonly SliceFixture _fixture;

        public AuthTestsHelper(SliceFixture fixture)
        {
            _fixture = fixture;
        }

        public async Task LoginAndCheckClaim(string email, string password, Func<Claim, bool> claimPredicate)
        {
            var loginResult = await _fixture.SendAsync(new Login
            {
                Email = email,
                Password = password
            });

            loginResult.Exists(jwt =>
            {
                var decoded = new JwtSecurityToken(jwt.TokenString);

                return decoded
                    .Claims
                    .Any(claimPredicate);
            })
            .ShouldBeTrue();
        }

        public async Task<JwtView> Login(string email, string password) =>
            (await _fixture.SendAsync(new Login
            {
                Email = email,
                Password = password
            }))
            .ValueOr(() => throw new InvalidOperationException("Tried to login with invalid credentials."));

        public Task<(string Email, string Password)> RegisterAdminAccountIfNotExisting() =>
            _fixture.ExecuteScopeAsync(serviceProvider =>
            {
                var userManager = (UserManager<User>)serviceProvider.GetService(typeof(UserManager<User>));
                var configuration = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));

                return DatabaseConfiguration.AddDefaultAdminAccountIfNoneExisting(userManager, configuration);
            });

        public async Task<string> GetAdminToken()
        {
            var (Email, Password) = await RegisterAdminAccountIfNotExisting();

            var token = (await Login(Email, Password))
                .TokenString;

            return token;
        }
    }
}
