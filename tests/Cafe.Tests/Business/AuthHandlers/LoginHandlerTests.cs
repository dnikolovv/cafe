using Cafe.Core.AuthContext.Commands;
using Cafe.Domain;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Shouldly;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.AuthHandlers
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
            result.Exists(jwt => IsValidJwtString(jwt.TokenString)).ShouldBeTrue();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotLoginWithInvalidCredentials(Register registerUserCommand)
        {
            // Arrange
            await _fixture.SendAsync(registerUserCommand);

            var loginCommand = new Login
            {
                Email = registerUserCommand.Email,
                Password = registerUserCommand.Password + "123" // Must be different
            };

            // Act
            var result = await _fixture.SendAsync(loginCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Unauthorized);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotLoginWithInvalidEmail(Login command)
        {
            // Arrange
            command.Email = "invalid-email";

            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.ValidationError);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotLoginWithUnexistingEmail(Login command)
        {
            // Arrange
            // We are not registering any users so the email is definitely not going to be found

            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        private static bool IsValidJwtString(string tokenString)
        {
            try
            {
                var decodedJwt = new JwtSecurityToken(tokenString);

                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
