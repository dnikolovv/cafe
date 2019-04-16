using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.AuthContext
{
    public class AssignWaiterToAccountHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly AuthTestsHelper _helper;

        public AssignWaiterToAccountHandlerTests()
        {
            _fixture = new SliceFixture();
            _helper = new AuthTestsHelper(_fixture);
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

            await _fixture.SendAsync(registerAccountCommand);

            var commandToTest = new AssignWaiterToAccount
            {
                WaiterId = waiterToAssign.Id,
                AccountId = registerAccountCommand.Id
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            await _helper.LoginAndCheckClaim(
                registerAccountCommand.Email,
                registerAccountCommand.Password,
                c => c.Type == AuthConstants.ClaimTypes.WaiterId &&
                     c.Value == waiterToAssign.Id.ToString());
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanReassignWaiterForAccount(Register registerAccountCommand, Waiter waiterToAssign, Waiter waiterToReassign)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Waiters.Add(waiterToAssign);
                dbContext.Waiters.Add(waiterToReassign);
                await dbContext.SaveChangesAsync();
            });

            await _fixture.SendAsync(registerAccountCommand);

            var assignFirstWaiterCommand = new AssignWaiterToAccount
            {
                WaiterId = waiterToAssign.Id,
                AccountId = registerAccountCommand.Id
            };

            // Note that first we've assigned a waiter before attempting a second time
            await _fixture.SendAsync(assignFirstWaiterCommand);

            var commandToTest = new AssignWaiterToAccount
            {
                AccountId = registerAccountCommand.Id,
                WaiterId = waiterToReassign.Id
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            await _helper.LoginAndCheckClaim(
                registerAccountCommand.Email,
                registerAccountCommand.Password,
                c => c.Type == AuthConstants.ClaimTypes.WaiterId &&
                     c.Value == waiterToReassign.Id.ToString());
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAssignUnexistingWaiter(Register registerAccountCommand)
        {
            // Arrange
            // Purposefully skipping adding any waiters
            var commandToTest = new AssignWaiterToAccount
            {
                WaiterId = Guid.NewGuid(),
                AccountId = registerAccountCommand.Id
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAssignUnexistingAccount(Waiter waiterToAdd)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Waiters.Add(waiterToAdd);
                await dbContext.SaveChangesAsync();
            });

            var commandToTest = new AssignWaiterToAccount
            {
                WaiterId = waiterToAdd.Id,
                AccountId = Guid.NewGuid()
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }
    }
}
