using Cafe.Core.AuthContext;
using Cafe.Core.ManagerContext.Commands;
using Cafe.Core.ManagerContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    [Authorize(Policy = AuthConstants.Policies.IsAdmin)]
    public class ManagerController : ApiController
    {
        private readonly IMediator _mediator;

        public ManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all currently employed managers.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEmployedManagers() =>
            (await _mediator.Send(new GetEmployedManagers()))
                .Match(Ok, Error);

        /// <summary>
        /// Hires a new manager.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> HireManager([FromBody] HireManager command) =>
            (await _mediator.Send(command))
                .Match(Ok, Error);
    }
}
