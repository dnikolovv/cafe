using AutoMapper;
using Cafe.Core.OrderContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.OrderContext.CommandHandlers
{
    public class ConfirmToGoOrderHandler : BaseHandler<ConfirmToGoOrder>
    {
        public ConfirmToGoOrderHandler(
            IValidator<ConfirmToGoOrder> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public override Task<Option<Unit, Error>> Handle(ConfirmToGoOrder command) =>
            OrderMustExist(command.OrderId).FlatMapAsync(order =>
            OrderMustBeUnconfirmed(order.Status).FlatMapAsync(currentStatus =>
            PaymentMustBeAtLeastWhatsOwed(order.OrderedItems, command.PricePaid).MapAsync(totalPrice =>
            SetStatusToConfirmed(order))));

        private Option<ToGoOrderStatus, Error> OrderMustBeUnconfirmed(ToGoOrderStatus currentStatus) =>
            currentStatus
                .SomeWhen(
                    status => status == ToGoOrderStatus.Unconfirmed,
                    Error.Validation($"You can only confirm unconfirmed orders."));

        private async Task<Option<ToGoOrder, Error>> OrderMustExist(Guid orderId) =>
            (await DbContext
                .ToGoOrders
                .Include(o => o.OrderedItems)
                .FirstOrDefaultAsync(o => o.Id == orderId))
            .SomeNotNull(Error.NotFound($"Order {orderId} was not found."));

        private Option<decimal, Error> PaymentMustBeAtLeastWhatsOwed(ICollection<MenuItem> orderedItems, decimal paymentAmount) =>
            orderedItems
                .Sum(i => i.Price)
                .SomeWhen(
                    total => paymentAmount >= total,
                    total => Error.Validation($"Tried to pay {paymentAmount} when the order price is {total}."));

        private async Task<Unit> SetStatusToConfirmed(ToGoOrder order)
        {
            order.Status = ToGoOrderStatus.Confirmed;
            DbContext.ToGoOrders.Update(order);
            await DbContext.SaveChangesAsync();
            return Unit.Value;
        }
    }
}