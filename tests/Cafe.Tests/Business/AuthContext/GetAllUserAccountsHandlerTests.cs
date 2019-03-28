using AutoFixture.Xunit2;
using Cafe.Core.AuthContext.Commands;
using Cafe.Core.AuthContext.Queries;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.AuthContext
{
    public class GetAllUserAccountsHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public GetAllUserAccountsHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [AutoData]
        public async Task CanGetAllUserAccounts(Register[] registerAccountsCommands)
        {
            // Arrange
            foreach (var command in registerAccountsCommands)
            {
                await _fixture.SendAsync(command);
            }

            var query = new GetAllUserAccounts();

            // Act
            var result = await _fixture.SendAsync(query);

            // Assert
            result.Exists(accounts =>
            {
                return accounts.All(a => registerAccountsCommands.Any(registeredAccount =>
                    a.Id == registeredAccount.Id &&
                    a.FirstName == registeredAccount.FirstName &&
                    a.LastName == registeredAccount.LastName &&
                    a.Email == registeredAccount.Email));
            })
            .ShouldBeTrue();
        }
    }
}
