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
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.OrderContext.CommandHandlers
{
    public class CompleteToGoOrderHandler : BaseOrderHandler<CompleteToGoOrder>
    {
        public CompleteToGoOrderHandler(
            IValidator<CompleteToGoOrder> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public override Task<Option<Unit, Error>> Handle(CompleteToGoOrder command) =>
            OrderMustExist(command.OrderId).FlatMapAsync(order =>
            OrderMustHaveStatus(ToGoOrderStatus.Issued, order).FlatMapAsync(currentStatus =>
            SetOrderStatus(ToGoOrderStatus.Completed, order).MapAsync(_ =>
            AssignOrderToBaristaIfAny(command.BaristaId, order))));

        private async Task<Unit> AssignOrderToBaristaIfAny(Option<Guid> baristaIdOption, ToGoOrder order)
        {
            await baristaIdOption.MapAsync(
                async baristaId =>
                {
                    var barista = await DbContext
                        .Baristas
                        .Include(b => b.CompletedOrders)
                        .FirstOrDefaultAsync(b => b.Id == baristaId);

                    if (barista == null)
                    {
                        throw new InvalidOperationException(
                           $"Tried to assign an order to an unexisting barista. (id: {baristaId})" +
                           $"It is very likely that the claim assigning logic is broken.");
                    }

                    barista.CompletedOrders.Add(order);
                    await DbContext.SaveChangesAsync();

                    return Unit.Value;
                });

            return Unit.Value;
        }
    }
}
