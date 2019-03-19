using Cafe.Core.Auth.Commands;
using Cafe.Tests.Customizations;
using Shouldly;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.Auth.Handlers
{
    public class LoginHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public LoginHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanLogin(Register registerUserCommand)
        {
            // Arrange
            await _fixture.SendAsync(registerUserCommand);

            var loginCommand = new Login
            {
                Email = registerUserCommand.Email,
                Password = registerUserCommand.Password
            };

            // Act
            var result = await _fixture.SendAsync(loginCommand);

            // Assert
            // TODO: Validate subject, claims, etc.
            result.Exists(jwt => IsValidJwt(jwt.TokenString)).ShouldBeTrue();
        }

        private static bool IsValidJwt(string tokenString)
        {
            try
            {
                var decodedJwt = new JwtSecurityToken(tokenString);

                return true;
            }
            catch (ArgumentException e)
            {
                return false;
            }
        }
    }
}
