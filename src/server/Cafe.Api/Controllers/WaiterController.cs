using Cafe.Core.AuthContext;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Core.WaiterContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    public class WaiterController : ApiController
    {
        private readonly IMediator _mediator;

        public WaiterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a list of all currently employed waiters in the café.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = AuthConstants.Policies.IsManager)]
        public async Task<IActionResult> GetEmployedWaiters() =>
            (await _mediator.Send(new GetEmployedWaiters()))
            .Match(Ok, Error);

        /// <summary>
        /// Hires a waiter in the café.
        /// </summary>
        [HttpPost("hire")]
        [Authorize(Policy = AuthConstants.Policies.IsManager)]
        public async Task<IActionResult> HireWaiter([FromBody] HireWaiter command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);

        /// <summary>
        /// Assigns a table to a waiter.
        /// </summary>
        [HttpPost("table/assign")]
        [Authorize(Policy = AuthConstants.Policies.IsManager)]
        public async Task<IActionResult> AssignTable([FromBody] AssignTable command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);
    }
}
