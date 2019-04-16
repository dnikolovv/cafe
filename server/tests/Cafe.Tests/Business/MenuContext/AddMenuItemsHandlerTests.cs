using Cafe.Core.MenuContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Views;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.MenuContext
{
    public class AddMenuItemsHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public AddMenuItemsHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanAddMenuItems(MenuItemView[] itemsToAdd)
        {
            // Arrange
            var command = new AddMenuItems
            {
                MenuItems = itemsToAdd
            };

            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            var itemsInDb = await _fixture.ExecuteDbContextAsync(dbContext => dbContext
                .MenuItems
                .ToListAsync());

            itemsInDb.ShouldAllBe(i => itemsToAdd.Any(addedItem =>
                i.Number == addedItem.Number &&
                i.Description == addedItem.Description &&
                i.Price == addedItem.Price));
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAddConflictingMenuItemsWhenAllAreConflicting(MenuItemView[] itemsToAdd)
        {
            // Arrange
            var command = new AddMenuItems
            {
                MenuItems = itemsToAdd
            };

            await _fixture.SendAsync(command);

            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Conflict);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotAddConflictingMenuItemsWhenSomeAreConflicting(AddMenuItems command)
        {
            // Arrange
            await _fixture.SendAsync(command);

            var commandToTest = new AddMenuItems
            {
                // Purposefully taking only the first item
                MenuItems = new List<MenuItemView> { command.MenuItems.First() }
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Conflict);
        }
    }
}
