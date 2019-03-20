using Cafe.Core.Tab.Commands;
using Cafe.Core.Tab.Queries;
using Cafe.Domain.Views;
using Cafe.Tests.Customizations;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Business.TabHandlers
{
    public class OpenTabHandlerTests : ResetDatabaseLifetime
    {
        private readonly SliceFixture _fixture;

        public OpenTabHandlerTests()
        {
            _fixture = new SliceFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanOpenTab(OpenTab command)
        {
            // Arrange
            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            await AssertTabExists(command.Id, t => t.IsOpen == true);
        }

        private async Task AssertTabExists(Guid tabId, Func<TabView, bool> predicate)
        {
            var tab = await _fixture.SendAsync(new GetTabView { Id = tabId });
            tab.Exists(predicate).ShouldBeTrue();
        }
    }
}
