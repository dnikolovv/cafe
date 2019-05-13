using Cafe.Api.Hateoas.Resources;
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
        public WaiterController(IResourceMapper resourceMapper, IMediator mediator)
            : base(resourceMapper, mediator)
        {
        }

        /// <summary>
        /// Retrieves a list of all currently employed waiters in the café.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrManager)]
        public async Task<IActionResult> GetEmployedWaiters() =>
            (await Mediator.Send(new GetEmployedWaiters()))
            .Match(Ok, Error);

        /// <summary>
        /// Hires a waiter in the café.
        /// </summary>
        [HttpPost("hire")]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrManager)]
        public async Task<IActionResult> HireWaiter([FromBody] HireWaiter command) =>
            (await Mediator.Send(command))
            .Match(Ok, Error);

        /// <summary>
        /// Assigns a table to a waiter.
        /// </summary>
        [HttpPost("table/assign")]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrManager)]
        public async Task<IActionResult> AssignTable([FromBody] AssignTable command) =>
            (await Mediator.Send(command))
            .Match(Ok, Error);
    }
}
