using Cafe.Core.WaiterContext.Commands;
using MediatR;
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
        /// Hires a waiter in the café.
        /// </summary>
        [HttpPost("hire")]
        public async Task<IActionResult> HireWaiter([FromBody] HireWaiter command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);

        /// <summary>
        /// Assigns a table to a waiter.
        /// </summary>
        [HttpPost("table/assign")]
        public async Task<IActionResult> AssignTable([FromBody] AssignTable command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);
    }
}
