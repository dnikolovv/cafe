using Cafe.Core.WaiterContext.Queries;
using Cafe.Domain.Entities;
using Cafe.Tests.Customizations;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.WaiterContext
{
    public class GetEmployedWaitersHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;

        public GetEmployedWaitersHandlerTests()
        {
            _fixture = new AppFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanGetAllEmployedWaiters(GetEmployedWaiters query, Waiter[] waitersToHire)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Waiters.AddRange(waitersToHire);
                await dbContext.SaveChangesAsync();
            });

            // Act
            var waiters = await _fixture.SendAsync(query);

            // Assert
            (waiters.Count == waitersToHire.Length &&
             waiters.All(w => waitersToHire.Any(hiredWaiter => w.Id == hiredWaiter.Id &&
                                                               w.ShortName == hiredWaiter.ShortName &&
                                                               w.TablesServed.Count == hiredWaiter.ServedTables.Count)))
            .ShouldBeTrue();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanGetAllEmployedWaitersWhenNoneHaveBeenHired(GetEmployedWaiters query)
        {
            // Arrange
            // Purposefully not hiring any waiters

            // Act
            var waiters = await _fixture.SendAsync(query);

            // Assert
            (waiters.Count == 0).ShouldBeTrue();
        }
    }
}
