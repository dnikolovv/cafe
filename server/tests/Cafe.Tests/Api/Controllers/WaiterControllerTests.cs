using AutoFixture;
using Cafe.Api.Hateoas;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Tests.Customizations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class WaiterControllerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly ApiTestsHelper _apiHelper;

        public WaiterControllerTests()
        {
            _fixture = new AppFixture();
            _apiHelper = new ApiTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task GetEmployedWaitersShouldReturnProperHypermediaLinks(HireWaiter[] waitersToHire, Fixture fixture) =>
            _apiHelper.InTheContextOfAManager(
                manager => async httpClient =>
                {
                    // Arrange
                    await _fixture.SendManyAsync(waitersToHire);

                    // Act
                    var response = await httpClient.GetAsync(WaiterRoute());

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Waiter.Hire
                    };

                    var resource = response.ShouldBeAResource<WaitersContainerResource>();

                    resource.Items.ShouldAllBe(r => r.Links.Count > 0);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public async Task HireWaiterShouldReturnProperHypermediaLinks(HireWaiter command, Fixture fixture) =>
            _apiHelper.InTheContextOfAManager(
                manager => async httpClient =>
                {
                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync(WaiterRoute("hire"), command);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Waiter.Get,
                        LinkNames.Waiter.GetEmployedWaiters,
                        LinkNames.Waiter.AssignTable
                    };

                    response.ShouldBeAResource<HireWaiter>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public async Task AssignTableShouldReturnProperHypermediaLinks(HireWaiter hireWaiterCommand, AddTable addTableCommand, Fixture fixture) =>
            _apiHelper.InTheContextOfAManager(
                manager => async httpClient =>
                {
                    // Arrange
                    await _fixture.SendAsync(hireWaiterCommand);
                    await _fixture.SendAsync(addTableCommand);

                    var assignTableCommand = new AssignTable
                    {
                        WaiterId = hireWaiterCommand.Id,
                        TableNumber = addTableCommand.Number
                    };

                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync(WaiterRoute("table/assign"), assignTableCommand);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Waiter.Get,
                        LinkNames.Waiter.GetEmployedWaiters,
                        LinkNames.Waiter.Hire
                    };

                    response.ShouldBeAResource<HireWaiter>(expectedLinks);
                },
                fixture);

        private static string WaiterRoute(string route = null) =>
            $"waiter/{route?.TrimStart('/') ?? string.Empty}";
    }
}
