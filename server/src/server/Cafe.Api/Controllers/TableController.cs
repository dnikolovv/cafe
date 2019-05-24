using Cafe.Api.Hateoas.Resources;
using Cafe.Api.Hateoas.Resources.Table;
using Cafe.Core.AuthContext;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.TableContext.Queries;
using Cafe.Domain.Views;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optional.Async;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    public class TableController : ApiController
    {
        public TableController(IResourceMapper resourceMapper, IMediator mediator)
            : base(resourceMapper, mediator)
        {
        }

        /// <summary>
        /// Retrieves a list of the tables in the café.
        /// </summary>
        [HttpGet(Name = nameof(GetAllTables))]
        public Task<IActionResult> GetAllTables() =>
            ResourceContainerResult<TableView, TableResource, TableContainerResource>(new GetAllTables());

        /// <summary>
        /// Adds a table to the café.
        /// </summary>
        [HttpPost(Name = nameof(AddTable))]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrManager)]
        public async Task<IActionResult> AddTable([FromBody] AddTable command) =>
            (await Mediator.Send(command)
                .MapAsync(ToEmptyResourceAsync<AddTableResource>))
                .Match(Ok, Error);

        /// <summary>
        /// Calls the waiter assigned to the table number provided.
        /// </summary>
        [Authorize]
        [HttpPost("{tableNumber}/callWaiter", Name = nameof(CallWaiter))]
        public async Task<IActionResult> CallWaiter(int tableNumber) =>
            (await Mediator.Send(new CallWaiter { TableNumber = tableNumber })
                .MapAsync(_ => ToEmptyResourceAsync<CallWaiterResource>(x => x.TableNumber = tableNumber)))
                .Match(Ok, Error);

        /// <summary>
        /// Requests the bill from the waiter assigned to the table number provided.
        /// </summary>
        [Authorize]
        [HttpPost("{tableNumber}/requestBill", Name = nameof(RequestBill))]
        public async Task<IActionResult> RequestBill(int tableNumber) =>
            (await Mediator.Send(new RequestBill { TableNumber = tableNumber })
                .MapAsync(_ => ToEmptyResourceAsync<RequestBillResource>(x => x.TableNumber = tableNumber)))
                .Match(Ok, Error);
    }
}
