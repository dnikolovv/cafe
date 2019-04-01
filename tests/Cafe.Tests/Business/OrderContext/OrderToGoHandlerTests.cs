using Cafe.Core.OrderContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Tests.Customizations;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.OrderContext
{
    public class OrderToGoHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public OrderToGoHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanOrderToGo(MenuItem[] menuItems)
        {
            // Arrange
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.MenuItems.AddRange(menuItems);
                await dbContext.SaveChangesAsync();
            });

            var menuItemNumbers = menuItems
                .Select(i => i.Number)
                .ToArray();

            var commandToTest = new OrderToGo
            {
                Id = Guid.NewGuid(),
                ItemNumbers = menuItemNumbers
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.HasValue.ShouldBeTrue();

            var orderInDb = await _fixture.ExecuteDbContextAsync(dbContext =>
                dbContext
                    .Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.Id == commandToTest.Id));

            orderInDb.ShouldNotBeNull();
            orderInDb.Items.ShouldAllBe(i => menuItemNumbers.Contains(i.Number));
        }
    }
}
