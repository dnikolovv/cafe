using AutoMapper;
using Cafe.Core.WaiterContext.Commands;
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

namespace Cafe.Business.WaiterContext.CommandHandlers
{
    public class HireWaiterHandler : BaseHandler<HireWaiter>
    {
        public HireWaiterHandler(
            IValidator<HireWaiter> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public override Task<Option<Unit, Error>> Handle(HireWaiter command) =>
            WaiterShouldntExist(command.Id).MapAsync(__ =>
            PersistWaiter(command));

        private async Task<Option<Unit, Error>> WaiterShouldntExist(Guid waiterId) =>
            (await DbContext
                .Waiters
                .FirstOrDefaultAsync(w => w.Id == waiterId))
                .SomeWhen(w => w == null, Error.Conflict($"Waiter {waiterId} already exists."))
                .Map(_ => Unit.Value);

        private async Task<Unit> PersistWaiter(HireWaiter command)
        {
            var waiter = Mapper.Map<Waiter>(command);

            await DbContext.AddAsync(waiter);
            await DbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
