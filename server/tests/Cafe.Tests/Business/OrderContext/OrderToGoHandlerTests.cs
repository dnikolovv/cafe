using Cafe.Core.OrderContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Tests.Business.OrderContext.Helpers;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.OrderContext
{
    public class OrderToGoHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly ToGoOrderTestsHelper _helper;

        public OrderToGoHandlerTests()
        {
            _fixture = new AppFixture();
            _helper = new ToGoOrderTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanOrderToGo(MenuItem[] menuItems)
        {
            // Arrange
            await _helper.AddMenuItems(menuItems);

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
            await _helper.AssertOrderExists(
                commandToTest.Id,
                order => order.Status == ToGoOrderStatus.Pending &&
                         order.OrderedItems.Count == menuItemNumbers.Length &&
                         order.OrderedItems.All(i => menuItemNumbers.Contains(i.Number)));
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanOrderMultiplesOfSameItems(MenuItem[] menuItems)
        {
            // Arrange
            const int timesToOrderEachItem = 3;

            await _helper.AddMenuItems(menuItems);

            var commandToTest = new OrderToGo
            {
                Id = Guid.NewGuid(),
                ItemNumbers = menuItems
                    .SelectMany(i => Enumerable.Repeat(i.Number, timesToOrderEachItem))
                    .ToList()
            };

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            await _helper.AssertOrderExists(commandToTest.Id, order =>
                order.OrderedItems.Count == menuItems.Length * timesToOrderEachItem &&
                menuItems.All(i => order.OrderedItems.Any(oi => oi.Number == i.Number)));
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanOrderToGoInSequence(MenuItem[] menuItems)
        {
            // Arrange
            await _helper.AddMenuItems(menuItems);

            var menuItemNumbers = menuItems
                .Select(i => i.Number)
                .ToArray();

            var commandsToTest = new List<OrderToGo>
            {
                new OrderToGo
                {
                    Id = Guid.NewGuid(),
                    ItemNumbers = menuItemNumbers
                },
                new OrderToGo
                {
                    Id = Guid.NewGuid(),
                    ItemNumbers = menuItemNumbers
                },
                new OrderToGo
                {
                    Id = Guid.NewGuid(),
                    ItemNumbers = menuItemNumbers
                }
            };

            // Act
            var results = (await Task
                .WhenAll(commandsToTest
                .Select(c => _fixture.SendAsync(c))))
                .Select(r => r);

            // Assert
            results.ShouldAllBe(r => r.HasValue);

            foreach (var commandToTest in commandsToTest)
            {
                await _helper.AssertOrderExists(
                    commandToTest.Id,
                    order => order.Status == ToGoOrderStatus.Pending &&
                             order.OrderedItems.Count == menuItemNumbers.Length &&
                             order.OrderedItems.All(i => menuItemNumbers.Contains(i.Number)));
            }
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotOrderUnexistingMenuItemsToGo(OrderToGo commandToTest)
        {
            // Arrange
            // Purposefully not adding any menu items

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.NotFound);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotSubmitToGoOrderWithConflictingId(MenuItem[] menuItems)
        {
            // Arrange
            await _helper.AddMenuItems(menuItems);

            var menuItemNumbers = menuItems
                .Select(i => i.Number)
                .ToArray();

            var commandToTest = new OrderToGo
            {
                Id = Guid.NewGuid(),
                ItemNumbers = menuItemNumbers
            };

            // Note that we're sending the command before the Act phase
            await _fixture.SendAsync(commandToTest);

            // Act
            var result = await _fixture.SendAsync(commandToTest);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Conflict);
        }
    }
}
