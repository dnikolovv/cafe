using Cafe.Core.AuthContext;
using Cafe.Core.CashierContext.Commands;
using Cafe.Core.CashierContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    public class CashierController : ApiController
    {
        private readonly IMediator _mediator;

        public CashierController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a list of all currently employed cashiers in the café.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrManager)]
        public async Task<IActionResult> GetEmployedCashiers() =>
            (await _mediator.Send(new GetEmployedCashiers()))
            .Match(Ok, Error);

        /// <summary>
        /// Hires a cashier in the café.
        /// </summary>
        [HttpPost("hire")]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrManager)]
        public async Task<IActionResult> HireCashier([FromBody] HireCashier command) =>
            (await _mediator.Send(command))
            .Match(Ok, Error);
    }
}
