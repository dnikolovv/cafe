using Cafe.Core.AuthContext;
using Cafe.Core.OrderContext.Commands;
using Cafe.Core.OrderContext.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    [Authorize(Policy = AuthConstants.Policies.IsAdminOrCashier)]
    public class OrderController : ApiController
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a to-go order by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id) =>
            (await _mediator.Send(new GetToGoOrder { Id = id }))
                .Match(Ok, NotFound);

        /// <summary>
        /// Opens a new to-go order.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> OrderToGo([FromBody] OrderToGo command) =>
            (await _mediator.Send(command))
                .Match(Ok, Error);

        /// <summary>
        /// Confirms a to-go order.
        /// </summary>
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmToGoOrder([FromBody] ConfirmToGoOrder command) =>
            (await _mediator.Send(command))
                .Match(Ok, Error);
    }
}
