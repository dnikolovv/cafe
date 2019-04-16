using Cafe.Core.AuthContext;
using Cafe.Core.OrderContext.Commands;
using Cafe.Core.OrderContext.Queries;
using Cafe.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
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
        [Authorize]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id) =>
            (await _mediator.Send(new GetToGoOrder { Id = id }))
                .Match(Ok, NotFound);

        /// <summary>
        /// Retrieves orders for a given status.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrdersByStatus(ToGoOrderStatus status) =>
            (await _mediator.Send(new GetOrdersByStatus { Status = status }))
                .Match(Ok, Error);

        /// <summary>
        /// Opens a new to-go order.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrCashier)]
        public async Task<IActionResult> OrderToGo([FromBody] OrderToGo command) =>
            (await _mediator.Send(command))
                .Match(Ok, Error);

        /// <summary>
        /// Confirms a to-go order.
        /// </summary>
        [HttpPut("confirm")]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrCashier)]
        public async Task<IActionResult> ConfirmToGoOrder([FromBody] ConfirmToGoOrder command) =>
            (await _mediator.Send(command))
                .Match(Ok, Error);

        /// <summary>
        /// Completes a to-go order.
        /// </summary>
        [HttpPut("complete")]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrBarista)]
        public async Task<IActionResult> CompleteToGoOrder([FromBody] CompleteToGoOrder command)
        {
            command.BaristaId = BaristaId;

            var result = (await _mediator.Send(command))
                .Match(Ok, Error);

            return result;
        }
    }
}
