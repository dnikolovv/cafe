using Cafe.Core.TableContext.Commands;
using Cafe.Domain;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.TableContext
{
    public class AddTableHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;

        public AddTableHandlerTests()
        {
            _fixture = new AppFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanAddTable(AddTable command)
        {
            // Arrange
            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            var tableInDb = await _fixture.ExecuteDbContextAsync(dbContext =>
                dbContext.Tables.FirstOrDefaultAsync(t => t.Id == command.Id));

            tableInDb.ShouldNotBeNull();
            tableInDb.Number.ShouldBe(command.Number);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAddTableWithATakenNumber(AddTable addFirstTableCommand)
        {
            // Arrange
            await _fixture.SendAsync(addFirstTableCommand);

            var addTableWithTheSameNumberCommand = new AddTable
            {
                Id = Guid.NewGuid(),
                Number = addFirstTableCommand.Number
            };

            // Act
            var result = await _fixture.SendAsync(addTableWithTheSameNumberCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Conflict);
        }
    }
}
