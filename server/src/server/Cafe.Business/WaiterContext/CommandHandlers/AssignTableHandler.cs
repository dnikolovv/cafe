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
    public class AssignTableHandler : BaseHandler<AssignTable>
    {
        public AssignTableHandler(
            IValidator<AssignTable> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public override Task<Option<Unit, Error>> Handle(AssignTable command) =>
            CheckIfTableExists(command.TableNumber).
            FilterAsync(async t => t.WaiterId != command.WaiterId, Error.Conflict($"Waiter {command.WaiterId} is already assigned to this table.")).FlatMapAsync(table =>
            CheckIfWaiterExists(command.WaiterId).FlatMapAsync(waiter =>
            AssignTable(table, waiter.Id)));

        private async Task<Option<Unit, Error>> AssignTable(Table table, Guid waiterToAssignToId)
        {
            table.WaiterId = waiterToAssignToId;
            await DbContext.SaveChangesAsync();
            return Unit.Value.Some<Unit, Error>();
        }

        private async Task<Option<Waiter, Error>> CheckIfWaiterExists(Guid waiterId)
        {
            var waiter = await DbContext
                .Waiters
                .FirstOrDefaultAsync(w => w.Id == waiterId);

            return waiter
                .SomeNotNull(Error.NotFound($"No waiter with an id of {waiterId} was found."));
        }

        private async Task<Option<Table, Error>> CheckIfTableExists(int tableNumber)
        {
            var table = await DbContext
                .Tables
                .FirstOrDefaultAsync(t => t.Number == tableNumber);

            return table
                .SomeNotNull(Error.NotFound($"No table with number {tableNumber} was found."));
        }
    }
}
