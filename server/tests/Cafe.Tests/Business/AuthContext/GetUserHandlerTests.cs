using Cafe.Core.AuthContext.Commands;
using Cafe.Core.AuthContext.Queries;
using Cafe.Core.CashierContext.Commands;
using Cafe.Core.WaiterContext.Commands;
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
        private readonly AppFixture _fixture;

        public GetUserHandlerTests()
        {
            _fixture = new AppFixture();
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
        public async Task AssignedRolesAreCorrectlyReturned(Register userToRegister, HireWaiter waiterToAssign, HireCashier cashierToAssign)
        {
            // Arrange
            await _fixture.SendAsync(userToRegister);
            await _fixture.SendAsync(waiterToAssign);
            await _fixture.SendAsync(cashierToAssign);

            var assignWaiterToAccount = new AssignWaiterToAccount
            {
                WaiterId = waiterToAssign.Id,
                AccountId = userToRegister.Id
            };

            var assignCashierToAccount = new AssignCashierToAccount
            {
                AccountId = userToRegister.Id,
                CashierId = cashierToAssign.Id
            };

            await _fixture.SendAsync(assignWaiterToAccount);
            await _fixture.SendAsync(assignCashierToAccount);

            // Act
            var result = await _fixture.SendAsync(new GetUser { Id = userToRegister.Id });

            // Assert
            result.Exists(u =>
                u.Id == userToRegister.Id &&
                u.WaiterId == waiterToAssign.Id &&
                u.CashierId == cashierToAssign.Id &&
                u.IsWaiter &&
                u.IsCashier &&
                u.BaristaId == null &&
                u.ManagerId == null &&
                !u.IsBarista &&
                !u.IsManager)
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
