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
            BaristaMustExist(command.BaristaId ?? Guid.Empty).FlatMapAsync(barista =>
            AssignOrderToBarista(barista, order).MapAsync(_ =>
            SetOrderStatus(ToGoOrderStatus.Completed, order)))));

        private async Task<Option<Barista, Error>> BaristaMustExist(Guid baristaId) =>
            (await DbContext
                .Baristas
                .Include(b => b.CompletedOrders)
                .FirstOrDefaultAsync(b => b.Id == baristaId))
            .SomeWhen(b => b != null, Error.NotFound($"Barista {baristaId} was not found."));

        private async Task<Option<Unit, Error>> AssignOrderToBarista(Barista barista, ToGoOrder order)
        {
            barista.CompletedOrders.Add(order);
            await DbContext.SaveChangesAsync();
            return Unit.Value.Some<Unit, Error>();
        }
    }
}
