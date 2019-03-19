using Cafe.Core.Auth.Commands;
using Cafe.Domain;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Optional.Unsafe;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.Auth.Handlers
{
    public class RegisterHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public RegisterHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanRegister(Register command)
        {
            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.HasValue.ShouldBeTrue();

            var userModel = result.ValueOrDefault();

            userModel.Id.ShouldNotBeNullOrEmpty();
            userModel.Email.ShouldBe(command.Email);
            userModel.FirstName.ShouldBe(command.FirstName);
            userModel.LastName.ShouldBe(command.LastName);

            var userInDb = await _fixture
                .ExecuteDbContextAsync(context => context.Users.FirstOrDefaultAsync(u => u.Id == userModel.Id));

            userInDb.ShouldNotBeNull();
            userInDb.Email.ShouldBe(command.Email);
            userInDb.FirstName.ShouldBe(command.FirstName);
            userInDb.LastName.ShouldBe(command.LastName);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotRegisterWithInvalidEmail(Register command)
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
        public async Task CannotRegisterTheSameEmailTwice(Register command)
        {
            // Arrange
            // First register
            await _fixture.SendAsync(command);

            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Conflict);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotRegisterWithMissingNames(Register command)
        {
            // Arrange
            command.FirstName = null;
            command.LastName = null;

            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.ValidationError);
        }
    }
}
