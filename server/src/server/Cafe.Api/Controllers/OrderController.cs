using Cafe.Api.Hateoas.Resources;
using Cafe.Api.Hateoas.Resources.Order;
using Cafe.Core;
using Cafe.Core.AuthContext;
using Cafe.Core.OrderContext.Commands;
using Cafe.Core.OrderContext.Queries;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optional.Async;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Api.Controllers
{
    public class OrderController : ApiController
    {
        public OrderController(IResourceMapper resourceMapper, IMediator mediator)
            : base(resourceMapper, mediator)
        {
        }

        /// <summary>
        /// Retrieves a to-go order by id.
        /// </summary>
        [HttpGet("{id}", Name = nameof(GetOrder))]
        [Authorize]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id) =>
            (await Mediator.Send(new GetToGoOrder { Id = id })
                .MapAsync(ToResourceAsync<ToGoOrderView, ToGoOrderResource>))
                .Match(Ok, NotFound);

        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        [HttpGet(Name = nameof(GetAllOrders))]
        [Authorize]
        public Task<IActionResult> GetAllOrders([FromQuery] ToGoOrderStatus? status)
        {
            // TODO: Implement proper filters on the get all endpoints
            IQuery<IList<ToGoOrderView>> request = status == null ?
                (IQuery<IList<ToGoOrderView>>)new GetAllToGoOrders() :
                new GetOrdersByStatus { Status = status.Value };

            return ResourceContainerResult<ToGoOrderView, ToGoOrderResource, ToGoOrderContainerResource>(request);
        }

        /// <summary>
        /// Opens a new to-go order.
        /// </summary>
        [HttpPost(Name = nameof(OrderToGo))]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrCashier)]
        public async Task<IActionResult> OrderToGo([FromBody] OrderToGo command) =>
            (await Mediator.Send(command)
                .MapAsync(_ => ToEmptyResourceAsync<OrderToGoResource>(x => x.OrderId = command.Id)))
                .Match(Ok, Error);

        /// <summary>
        /// Confirms a to-go order.
        /// </summary>
        [HttpPut("confirm", Name = nameof(ConfirmToGoOrder))]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrCashier)]
        public async Task<IActionResult> ConfirmToGoOrder([FromBody] ConfirmToGoOrder command) =>
            (await Mediator.Send(command)
                .MapAsync(_ => ToEmptyResourceAsync<ConfirmToGoOrderResource>(x => x.OrderId = command.OrderId)))
                .Match(Ok, Error);

        /// <summary>
        /// Completes a to-go order.
        /// </summary>
        [HttpPut("complete", Name = nameof(CompleteToGoOrder))]
        [Authorize(Policy = AuthConstants.Policies.IsAdminOrBarista)]
        public async Task<IActionResult> CompleteToGoOrder([FromBody] CompleteToGoOrder command)
        {
            command.BaristaId = BaristaId;

            var result = (await Mediator.Send(command)
                .MapAsync(_ => ToEmptyResourceAsync<CompleteToGoOrderResource>(x => x.OrderId = command.OrderId)))
                .Match(Ok, Error);

            return result;
        }
    }
}
