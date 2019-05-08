using Cafe.Core.AuthContext;
using Cafe.Core.TableContext.Commands;
using Cafe.Core.TableContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    public class TableController : ApiController
    {
        private readonly IMediator _mediator;

        public TableController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a list of the tables in the café.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTables() =>
            (await _mediator.Send(new GetAllTables()))
            .Match(Ok, Error);

        /// <summary>
        /// Adds a table to the café.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrManager)]
        public async Task<IActionResult> AddTable([FromBody] AddTable command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);

        /// <summary>
        /// Calls the waiter assigned to the table number provided.
        /// </summary>
        [Authorize]
        [HttpPost("{tableNumber}/callWaiter")]
        public async Task<IActionResult> CallWaiter(int tableNumber) =>
            (await _mediator.Send(new CallWaiter { TableNumber = tableNumber }))
            .Match(Ok, Error);

        /// <summary>
        /// Requests the bill from the waiter assigned to the table number provided.
        /// </summary>
        [Authorize]
        [HttpPost("{tableNumber}/requestBill")]
        public async Task<IActionResult> RequestBill(int tableNumber) =>
            (await _mediator.Send(new RequestBill { TableNumber = tableNumber }))
            .Match(Ok, Error);
    }
}
