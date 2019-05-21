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
    public class GetTabHistoryHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly TabTestsHelper _helper;

        public GetTabHistoryHandlerTests()
        {
            _fixture = new AppFixture();
            _helper = new TabTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanGetTabHistory(Guid[] tabIds)
        {
            // Arrange
            await CreateClosedTabsFromIds(tabIds);

            // Act
            var result = await _fixture.SendAsync(new GetTabHistory());

            // Assert
            result.Exists(tabs =>
                tabs.Count == tabIds.Length &&
                tabs.All(t => tabIds.Contains(t.Id)))
            .ShouldBeTrue();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task GetTabHistoryReturnsOnlyClosedTabs(Guid[] tabIds)
        {
            // Arrange
            var tabIdsToClose = tabIds.Skip(1).ToArray();

            await CreateClosedTabsFromIds(tabIdsToClose);

            // Leaving one tab open
            var openTabId = tabIds.First();

            await _helper.OpenTabOnTable(openTabId, 100);

            // Act
            var result = await _fixture.SendAsync(new GetTabHistory());

            // Assert
            // Since we've left the first open we expect only the rest to show
            result.Exists(tabs =>
                tabs.Count == tabIds.Length - 1 &&
                tabs.All(t => tabIdsToClose.Contains(t.Id)))
            .ShouldBeTrue();
        }

        private async Task CreateClosedTabsFromIds(Guid[] tabIds)
        {
            for (int i = 0; i < tabIds.Length; i++)
            {
                var tabId = tabIds[i];

                await _helper.OpenTabOnTable(tabId, tableNumber: i + 1);
                await _helper.CloseTab(tabId, 0);
            }
        }
    }
}