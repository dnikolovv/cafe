using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Tests.Customizations;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
