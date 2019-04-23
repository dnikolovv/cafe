using AutoMapper;
using Cafe.Core;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;
using System;
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.OrderContext.CommandHandlers
{
    public abstract class BaseOrderHandler<TCommand> : BaseHandler<TCommand>
        where TCommand : ICommand
    {
        public BaseOrderHandler(
            IValidator<TCommand> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        protected Option<ToGoOrderStatus, Error> OrderMustHaveStatus(ToGoOrderStatus expectedStatus, ToGoOrder order) =>
            order
                .Status
                .SomeWhen(
                    status => status == expectedStatus,
                    Error.Validation($"The order must have a status of '{Enum.GetName(typeof(ToGoOrderStatus), expectedStatus)}'."));

        protected async Task<Option<ToGoOrder, Error>> OrderMustExist(Guid orderId) =>
            (await DbContext
                .ToGoOrders
                .Include(o => o.OrderedItems)
                    .ThenInclude(i => i.MenuItem)
                .FirstOrDefaultAsync(o => o.Id == orderId))
            .SomeNotNull(Error.NotFound($"Order {orderId} was not found."));

        protected async Task<Option<Unit, Error>> SetOrderStatus(ToGoOrderStatus status, ToGoOrder order)
        {
            order.Status = status;
            DbContext.ToGoOrders.Update(order);
            await DbContext.SaveChangesAsync();
            return Unit.Value.Some<Unit, Error>();
        }
    }
}
