using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Tests.Customizations;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.AuthContext
{
    public class AssignCashierToAccountHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly AuthTestsHelper _helper;

        public AssignCashierToAccountHandlerTests()
        {
            _fixture = new SliceFixture();
            _helper = new AuthTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanAssignCashierToAccount(Register accountToRegisterCommand, Cashier cashierToAssign)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Cashiers.Add(cashierToAssign);
                await dbContext.SaveChangesAsync();
            });

            await _fixture.SendAsync(accountToRegisterCommand);

            var commandToTest = new AssignCashierToAccount
            {
                CashierId = cashierToAssign.Id,
                AccountId = accountToRegisterCommand.Id
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            await _helper.LoginAndCheckClaim(
                accountToRegisterCommand.Email,
                accountToRegisterCommand.Password,
                c => c.Type == AuthConstants.ClaimTypes.CashierId &&
                     c.Value == cashierToAssign.Id.ToString());
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanReassignCashierForAccount(Register registerAccountCommand, Cashier cashierToAssign, Cashier cashierToReassign)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Cashiers.Add(cashierToAssign);
                dbContext.Cashiers.Add(cashierToReassign);
                await dbContext.SaveChangesAsync();
            });

            await _fixture.SendAsync(registerAccountCommand);

            var assignFirstCashierCommand = new AssignCashierToAccount
            {
                CashierId = cashierToAssign.Id,
                AccountId = registerAccountCommand.Id
            };

            // Note that first we've assigned a cashier before attempting a second time
            await _fixture.SendAsync(assignFirstCashierCommand);

            var commandToTest = new AssignCashierToAccount
            {
                AccountId = registerAccountCommand.Id,
                CashierId = cashierToReassign.Id
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            await _helper.LoginAndCheckClaim(
                registerAccountCommand.Email,
                registerAccountCommand.Password,
                c => c.Type == AuthConstants.ClaimTypes.CashierId &&
                     c.Value == cashierToReassign.Id.ToString());
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAssignUnexistingCashier(Register registerAccountCommand)
        {
            // Arrange
            // Purposefully skipping adding any cashiers
            var commandToTest = new AssignCashierToAccount
            {
                CashierId = Guid.NewGuid(),
                AccountId = registerAccountCommand.Id
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAssignUnexistingAccount(Cashier cashierToAdd)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Cashiers.Add(cashierToAdd);
                await dbContext.SaveChangesAsync();
            });

            var commandToTest = new AssignCashierToAccount
            {
                CashierId = cashierToAdd.Id,
                AccountId = Guid.NewGuid()
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }
    }
}
