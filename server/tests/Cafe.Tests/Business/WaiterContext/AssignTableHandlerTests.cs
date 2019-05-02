using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.WaiterContext
{
    public class AssignTableHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public AssignTableHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanAssignTable(HireWaiter hireWaiterCommand, AddTable addTableCommand)
        {
            // Arrange
            await _fixture.SendAsync(addTableCommand);
            await _fixture.SendAsync(hireWaiterCommand);

            var assignTableCommand = new AssignTable
            {
                WaiterId = hireWaiterCommand.Id,
                TableNumber = addTableCommand.Number
            };

            // Act
            var result = await _fixture.SendAsync(assignTableCommand);

            // Assert
            var tableInDb = await _fixture.ExecuteDbContextAsync(dbContext =>
                dbContext
                    .Tables
                    .Include(t => t.Waiter)
                        .ThenInclude(w => w.ServedTables)
                    .FirstOrDefaultAsync(t => t.Id == addTableCommand.Id));

            tableInDb.ShouldNotBeNull();
            tableInDb.WaiterId.ShouldBe(hireWaiterCommand.Id);
            tableInDb.Waiter.ShouldNotBeNull();
            tableInDb.Waiter.ShortName.ShouldBe(hireWaiterCommand.ShortName);
            tableInDb.Waiter.ServedTables.ShouldContain(t => t.Id == addTableCommand.Id &&
                                                             t.Number == assignTableCommand.TableNumber);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAssignTheSameWaiterTwice(HireWaiter hireWaiterCommand, AddTable addTableCommand)
        {
            // Arrange
            await _fixture.SendAsync(addTableCommand);
            await _fixture.SendAsync(hireWaiterCommand);

            var assignTableCommand = new AssignTable
            {
                WaiterId = hireWaiterCommand.Id,
                TableNumber = addTableCommand.Number
            };

            await _fixture.SendAsync(assignTableCommand);

            // Act
            var result = await _fixture.SendAsync(assignTableCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Conflict);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAssignUnexistingTable(HireWaiter hireWaiterCommand)
        {
            // Arrange
            await _fixture.SendAsync(hireWaiterCommand);

            var assignTableCommand = new AssignTable
            {
                WaiterId = hireWaiterCommand.Id,
                TableNumber = 1234 // Irrelevant since we haven't added any tables
            };

            // Act
            var result = await _fixture.SendAsync(assignTableCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAssignUnexistingWaiter(AddTable addTableCommand)
        {
            // Arrange
            await _fixture.SendAsync(addTableCommand);

            var assignTableCommand = new AssignTable
            {
                WaiterId = Guid.NewGuid(), // Irrelevant since we haven't hired any waiters
                TableNumber = addTableCommand.Number
            };

            // Act
            var result = await _fixture.SendAsync(assignTableCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }
    }
}
