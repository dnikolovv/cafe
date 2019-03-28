using Cafe.Core.TabContext.Queries;
using Cafe.Tests.Business.TabContext.Helpers;
using Cafe.Tests.Customizations;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.TabContext
{
    public class GetAllOpenTabsHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;
        private readonly TabTestsHelper _helper;

        public GetAllOpenTabsHandlerTests()
        {
            _fixture = new SliceFixture();
            _helper = new TabTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanGetAllOpenTabs(Guid[] tabIds)
        {
            // Arrange
            for (int i = 0; i < tabIds.Length; i++)
            {
                await _helper.OpenTabOnTable(tabIds[i], tableNumber: i + 1);
            }

            // Act
            var result = await _fixture.SendAsync(new GetAllOpenTabs());

            // Assert
            result.Exists(tabs =>
                tabs.Count == tabIds.Length &&
                tabs.All(t => tabIds.Contains(t.Id)))
            .ShouldBeTrue();
        }
    }
}
