using AutoFixture;
using Cafe.Api.Hateoas;
using Cafe.Api.Hateoas.Resources.Table;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Tests.Business.TabContext.Helpers;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class TableControllerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly ApiTestsHelper _apiHelper;
        private readonly TabTestsHelper _tabHelper;

        public TableControllerTests()
        {
            _fixture = new AppFixture();
            _apiHelper = new ApiTestsHelper(_fixture);
            _tabHelper = new TabTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public Task GetAllTablesReturnsProperHypermediaLinksForTablesWithoutWaiters(AddTable[] addTablesCommands, Fixture fixture) =>
            _apiHelper.InTheContextOfAnAuthenticatedUser(
                async httpClient =>
                {
                    // Arrange
                    await _fixture.SendManyAsync(addTablesCommands);

                    // Act
                    var response = await httpClient
                        .GetAsync(TableRoute());

                    // Assert
                    var expectedContainerLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Table.Add
                    };

                    var resource = await response.ShouldBeAResource<TableContainerResource>(expectedContainerLinks);

                    var expectedTableLinks = new List<string>
                    {
                        LinkNames.Table.GetAll
                    };

                    resource.Items.ShouldAllBe(tableResource => tableResource.Links.All(link => expectedTableLinks.Contains(link.Key)));
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task GetAllTablesReturnsProperHypermediaLinksForTablesWithWaiters(Fixture fixture) =>
            _apiHelper.InTheContextOfAnAuthenticatedUser(
                async httpClient =>
                {
                    // Arrange
                    const int countOfTables = 5;

                    var hireWaiterCommands = fixture
                        .CreateMany<HireWaiter>(countOfTables)
                        .ToArray();

                    var addTableCommands = fixture
                        .CreateMany<AddTable>(countOfTables)
                        .ToArray();

                    for (int i = 0; i < hireWaiterCommands.Length; i++)
                    {
                        await _tabHelper.SetupWaiterWithTable(hireWaiterCommands[i], addTableCommands[i]);
                    }

                    // Act
                    var response = await httpClient
                        .GetAsync(TableRoute());

                    // Assert
                    var expectedContainerLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Table.Add
                    };

                    var resource = await response.ShouldBeAResource<TableContainerResource>(expectedContainerLinks);

                    var expectedTableLinks = new List<string>
                    {
                        LinkNames.Table.GetAll,
                        LinkNames.Table.CallWaiter,
                        LinkNames.Table.RequestBill
                    };

                    resource.Items.ShouldAllBe(tableResource => tableResource.Links.All(link => expectedTableLinks.Contains(link.Key)));
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task AddTableShouldReturnProperHypermediaLinks(AddTable command, Fixture fixture) =>
            _apiHelper.InTheContextOfAManager(
                manager => async httpClient =>
                {
                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync(TableRoute(), command);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Table.GetAll
                    };

                    await response.ShouldBeAResource<AddTableResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task CallWaiterShouldReturnProperHypermediaLinks(HireWaiter hireWaiter, AddTable addTable, Fixture fixture) =>
            _apiHelper.InTheContextOfAnAuthenticatedUser(
                async httpClient =>
                {
                    // Arrange
                    await _tabHelper.SetupWaiterWithTable(hireWaiter, addTable);

                    var callWaiter = new CallWaiter
                    {
                        TableNumber = addTable.Number
                    };

                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync(TableRoute($"{addTable.Number}/callWaiter"), callWaiter);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Table.RequestBill,
                        LinkNames.Table.GetAll
                    };

                    await response.ShouldBeAResource<CallWaiterResource>(expectedLinks);
                },
                fixture);

        [Theory]
        [CustomizedAutoData]
        public Task RequestBillShouldReturnProperHypermediaLinks(HireWaiter hireWaiter, AddTable addTable, Fixture fixture) =>
            _apiHelper.InTheContextOfAnAuthenticatedUser(
                async httpClient =>
                {
                    // Arrange
                    await _tabHelper.SetupWaiterWithTable(hireWaiter, addTable);

                    var requestBill = new RequestBill
                    {
                        TableNumber = addTable.Number
                    };

                    // Act
                    var response = await httpClient
                        .PostAsJsonAsync(TableRoute($"{addTable.Number}/requestBill"), requestBill);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Table.CallWaiter,
                        LinkNames.Table.GetAll
                    };

                    await response.ShouldBeAResource<RequestBillResource>(expectedLinks);
                },
                fixture);

        private static string TableRoute(string route = null) =>
            $"table/{route?.TrimStart('/') ?? string.Empty}";
    }
}
