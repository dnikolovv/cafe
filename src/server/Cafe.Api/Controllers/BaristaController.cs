using Cafe.Core.AuthContext;
using Cafe.Core.BaristaContext.Commands;
using Cafe.Core.BaristaContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    [Authorize(Policy = AuthConstants.Policies.IsAdminOrBarista)]
    public class BaristaController : ApiController
    {
        private readonly IMediator _mediator;

        public BaristaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Hires a new barista.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> HireBarista(HireBarista command) =>
            (await _mediator.Send(command))
                .Match(Ok, Error);

        /// <summary>
        /// Retrieves currently employed baristas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEmployedBaristas() =>
            (await _mediator.Send(new GetEmployedBaristas()))
                .Match(Ok, Error);
    }
}
