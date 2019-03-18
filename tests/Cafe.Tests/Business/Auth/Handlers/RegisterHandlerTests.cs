using AutoFixture.Xunit2;
using Cafe.Core.Auth.Commands;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.Auth.Handlers
{
    public class RegisterHandlerTests
    {
        private readonly SliceFixture _fixture;

        public RegisterHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [AutoData]
        public async Task CanRegister(Register command)
        {
            // Arrange
            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.Exists(user =>
                !string.IsNullOrEmpty(user.Id) &&
                user.Email == command.Email &&
                user.FirstName == command.FirstName &&
                user.LastName == command.LastName)
                .ShouldBeTrue();
        }
    }
}
