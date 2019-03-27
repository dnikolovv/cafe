using Cafe.Tests.Customizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.ManagerContext
{
    public class HireManagerHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public HireManagerHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanHireManager(HireManager command)
        {
            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            var managerInDb = await _fixture.ExecuteDbContextAsync(dbContext =>
            {
                var manager = dbContext
                    .Managers
                    .SingleAsync(m => m.Id == command.Id);

                return manager;
            });

            managerInDb.Id.ShouldBe(command.Id);
            managerInDb.FirstName.ShouldBe(command.FirstName);
            managerInDb.LastName.ShouldBe(command.LastName);
            managerInDb.ShortName.ShouldBe(command.ShortName);
        }
    }
}
