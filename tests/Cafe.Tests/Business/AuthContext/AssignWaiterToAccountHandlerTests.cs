using Cafe.Core.AuthContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Tests.Customizations;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.AuthContext
{
    public class AssignWaiterToAccountHandlerTests
    {
        private readonly SliceFixture _fixture;

        public AssignWaiterToAccountHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanAssignWaiterToAccount(Register registerAccountCommand, Waiter waiterToAssign)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Waiters.Add(waiterToAssign);
                await dbContext.SaveChangesAsync();
            });

            var registerAccountResult = await _fixture.SendAsync(registerAccountCommand);

            var registeredUser = await _fixture
                .ExecuteDbContextAsync(dbContext => dbContext
                    .Users
                    .SingleAsync(u => u.Email == registerAccountCommand.Email));

            var commandToTest = new AssignWaiterToAccount
            {
                WaiterId = waiterToAssign.Id,
                AccountId = registeredUser.Id
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            // We're going to check if the operation is successfull by validating whether the returned JWT from the login has a proper waiter claim
            var loginResult = await _fixture.SendAsync(new Login
            {
                Email = registerAccountCommand.Email,
                Password = registerAccountCommand.Password
            });

            loginResult.Exists(jwt =>
            {
                var decoded = new JwtSecurityToken(jwt.TokenString);

                // TODO: Get rid of magic strings
                return decoded
                    .Claims
                    .Any(c => c.Type == "waiterId" &&
                              c.Value == waiterToAssign.Id.ToString());
            })
            .ShouldBeTrue();
        }
    }
}
