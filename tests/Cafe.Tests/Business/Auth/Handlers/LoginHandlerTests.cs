using AutoFixture.Xunit2;
using Cafe.Core.Auth.Commands;
using Cafe.Tests.Customizations;
using Shouldly;
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
            // TODO: Validate if it's a real jwt with all of the claims properly set up
            result.Exists(jwt => !string.IsNullOrEmpty(jwt.TokenString)).ShouldBeTrue();
        }
    }
}
