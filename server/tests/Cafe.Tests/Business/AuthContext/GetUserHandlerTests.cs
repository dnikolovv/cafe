using Cafe.Core.AuthContext.Commands;
using Cafe.Core.AuthContext.Queries;
using Cafe.Domain;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.AuthContext
{
    public class GetUserHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public GetUserHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanQueryForUser(Register userToRegister)
        {
            // Arrange
            await _fixture.SendAsync(userToRegister);

            var queryToTest = new GetUser
            {
                Id = userToRegister.Id
            };

            // Act
            var result = await _fixture.SendAsync(queryToTest);

            // Assert
            result.Exists(u =>
                u.Id == userToRegister.Id &&
                u.FirstName == userToRegister.FirstName &&
                u.LastName == userToRegister.LastName &&
                u.Email == userToRegister.Email)
            .ShouldBeTrue();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task QueryingForAnUnexistingUserReturnsNotFound(GetUser query)
        {
            // Arrange
            // Purposefully not registering any users

            // Act
            var result = await _fixture.SendAsync(query);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }
    }
}
