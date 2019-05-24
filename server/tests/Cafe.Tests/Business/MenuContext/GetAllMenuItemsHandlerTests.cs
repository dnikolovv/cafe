using Cafe.Core.MenuContext.Queries;
using Cafe.Domain.Entities;
using Cafe.Tests.Business.TabContext.Helpers;
using Cafe.Tests.Customizations;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.MenuContext
{
    public class GetAllMenuItemsHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly TabTestsHelper _helper;

        public GetAllMenuItemsHandlerTests()
        {
            _fixture = new AppFixture();
            _helper = new TabTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanGetAllMenuItems(GetAllMenuItems query, MenuItem[] menuItems)
        {
            // Arrange
            await _helper.AddMenuItems(menuItems);

            // Act
            var items = await _fixture.SendAsync(query);

            // Assert
            (items.Count == menuItems.Length &&
             items.All(i => menuItems.Any(mi => i.Number == mi.Number &&
                                                i.Description == mi.Description &&
                                                i.Price == mi.Price)))
            .ShouldBeTrue();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanGetAllMenuItemsWhenNoneAreExisting(GetAllMenuItems query)
        {
            // Arrange
            // Purposefully skipping adding any items

            // Act
            var items = await _fixture.SendAsync(query);

            // Assert
            (items.Count == 0).ShouldBeTrue();
        }
    }
}
