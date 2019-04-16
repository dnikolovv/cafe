using Cafe.Core.AuthContext.Commands;
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
    }
}
