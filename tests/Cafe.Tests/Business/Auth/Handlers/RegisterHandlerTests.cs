using Cafe.Core.Auth.Commands;
using Cafe.Tests.Customizations;
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
    }
}
