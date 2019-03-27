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
    public class AssignManagerToAccountHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly AuthTestsHelper _helper;

        public AssignManagerToAccountHandlerTests()
        {
            _fixture = new SliceFixture();
            _helper = new AuthTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanAssignManagerToAccount(Register accountToRegisterCommand, Manager managerToAssign)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Managers.Add(managerToAssign);
                await dbContext.SaveChangesAsync();
            });

            await _fixture.SendAsync(accountToRegisterCommand);

            var commandToTest = new AssignManagerToAccount
            {
                ManagerId = managerToAssign.Id,
                AccountId = accountToRegisterCommand.Id
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            await _helper.LoginAndCheckClaim(
                accountToRegisterCommand.Email,
                accountToRegisterCommand.Password,
                c => c.Type == AuthConstants.ManagerIdClaimType &&
                     c.Value == managerToAssign.Id.ToString());
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanReassignManagerForAccount(Register registerAccountCommand, Manager managerToAssign, Manager managerToReassign)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Managers.Add(managerToAssign);
                dbContext.Managers.Add(managerToReassign);
                await dbContext.SaveChangesAsync();
            });

            await _fixture.SendAsync(registerAccountCommand);

            var assignFirstManagerCommand = new AssignManagerToAccount
            {
                ManagerId = managerToAssign.Id,
                AccountId = registerAccountCommand.Id
            };

            // Note that first we've assigned a manager before attempting a second time
            await _fixture.SendAsync(assignFirstManagerCommand);

            var commandToTest = new AssignManagerToAccount
            {
                AccountId = registerAccountCommand.Id,
                ManagerId = managerToReassign.Id
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            await _helper.LoginAndCheckClaim(
                registerAccountCommand.Email,
                registerAccountCommand.Password,
                c => c.Type == AuthConstants.ManagerIdClaimType &&
                     c.Value == managerToReassign.Id.ToString());
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAssignUnexistingManager(Register registerAccountCommand)
        {
            // Arrange
            // Purposefully skipping adding any managers
            var commandToTest = new AssignManagerToAccount
            {
                ManagerId = Guid.NewGuid(),
                AccountId = registerAccountCommand.Id
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAssignUnexistingAccount(Manager managerToAdd)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Managers.Add(managerToAdd);
                await dbContext.SaveChangesAsync();
            });

            var commandToTest = new AssignManagerToAccount
            {
                ManagerId = managerToAdd.Id,
                AccountId = Guid.NewGuid()
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }
    }
}
