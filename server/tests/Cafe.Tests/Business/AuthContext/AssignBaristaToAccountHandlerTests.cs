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
    public class AssignBaristaToAccountHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly AuthTestsHelper _helper;

        public AssignBaristaToAccountHandlerTests()
        {
            _fixture = new AppFixture();
            _helper = new AuthTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanAssignBaristaToAccount(Register accountToRegisterCommand, Barista baristaToAssign)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Baristas.Add(baristaToAssign);
                await dbContext.SaveChangesAsync();
            });

            await _fixture.SendAsync(accountToRegisterCommand);

            var commandToTest = new AssignBaristaToAccount
            {
                BaristaId = baristaToAssign.Id,
                AccountId = accountToRegisterCommand.Id
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            await _helper.LoginAndCheckClaim(
                accountToRegisterCommand.Email,
                accountToRegisterCommand.Password,
                c => c.Type == AuthConstants.ClaimTypes.BaristaId &&
                     c.Value == baristaToAssign.Id.ToString());
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanReassignBaristaForAccount(Register registerAccountCommand, Barista baristaToAssign, Barista baristaToReassign)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Baristas.Add(baristaToAssign);
                dbContext.Baristas.Add(baristaToReassign);
                await dbContext.SaveChangesAsync();
            });

            await _fixture.SendAsync(registerAccountCommand);

            var assignFirstBaristaCommand = new AssignBaristaToAccount
            {
                BaristaId = baristaToAssign.Id,
                AccountId = registerAccountCommand.Id
            };

            // Note that first we've assigned a barista before attempting a second time
            await _fixture.SendAsync(assignFirstBaristaCommand);

            var commandToTest = new AssignBaristaToAccount
            {
                AccountId = registerAccountCommand.Id,
                BaristaId = baristaToReassign.Id
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            await _helper.LoginAndCheckClaim(
                registerAccountCommand.Email,
                registerAccountCommand.Password,
                c => c.Type == AuthConstants.ClaimTypes.BaristaId &&
                     c.Value == baristaToReassign.Id.ToString());
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAssignUnexistingBarista(Register registerAccountCommand)
        {
            // Arrange
            // Purposefully skipping adding any baristas
            var commandToTest = new AssignBaristaToAccount
            {
                BaristaId = Guid.NewGuid(),
                AccountId = registerAccountCommand.Id
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAssignUnexistingAccount(Barista baristaToAdd)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Baristas.Add(baristaToAdd);
                await dbContext.SaveChangesAsync();
            });

            var commandToTest = new AssignBaristaToAccount
            {
                BaristaId = baristaToAdd.Id,
                AccountId = Guid.NewGuid()
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }
    }
}
