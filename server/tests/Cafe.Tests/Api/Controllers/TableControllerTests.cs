using AutoFixture;
using Cafe.Api.Hateoas;
using Cafe.Core.TableContext.Commands;
using Cafe.Tests.Customizations;
using Cafe.Tests.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests.Api.Controllers
{
    public class TableControllerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;
        private readonly ApiTestsHelper _apiHelper;

        public TableControllerTests()
        {
            _fixture = new AppFixture();
            _apiHelper = new ApiTestsHelper(_fixture);
        }

        [Theory]
        [CustomizedAutoData]
        public Task GetAllTablesReturnsProperHypermediaLinks(AddTable[] addTablesCommands, Fixture fixture) =>
            _apiHelper.InTheContextOfAnAuthenticatedUser(
                async httpClient =>
                {
                    // Arrange
                    await _fixture.SendManyAsync(addTablesCommands);

                    // Act
                    var response = await httpClient
                        .GetAsync(TableRoute());

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Table.Add
                    };

                    var resource = response.ShouldBeAResource<TableContainerResource>(expectedLinks);

                    // TODO:
                    resource.Items.ShouldAllBe(tableResource => tableResource.Links.Should);
                });

        [Theory]
        [CustomizedAutoData]
        public Task AddTableShouldReturnProperHypermediaLinks(AddTable command, Fixture fixture) =>
            _apiHelper.InTheContextOfAManager(
                manager => async httpClient =>
                {
                    // Act
                    var response = await httpClient
                        .PostAsync(TableRoute(), command);

                    // Assert
                    var expectedLinks = new List<string>
                    {
                        LinkNames.Self,
                        LinkNames.Table.GetAll
                    };

                    response.ShouldBeAResource<AddTableResource>(expectedLinks);
                },
                fixture);

        private static string TableRoute(string route = null) =>
            $"table/{route.TrimStart('/') ?? string.Empty}";
    }
}
