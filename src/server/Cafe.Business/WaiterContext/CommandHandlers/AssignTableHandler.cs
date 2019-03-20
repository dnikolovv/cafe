using AutoMapper;
using Cafe.Core;
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
using System.Threading;
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.WaiterContext.CommandHandlers
{
    public class AssignTableHandler : BaseHandler<AssignTable>, ICommandHandler<AssignTable>
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

        public Task<Option<Unit, Error>> Handle(AssignTable command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            CheckIfTableIsNotTaken(command.TableNumber, command.WaiterToAssignToId).FlatMapAsync(table =>
            AssignTable(table, command.WaiterToAssignToId)));

        private async Task<Option<Unit, Error>> AssignTable(Table table, Guid waiterToAssignToId)
        {
            table.WaiterId = waiterToAssignToId;
            await DbContext.SaveChangesAsync();
            return Unit.Value.Some<Unit, Error>();
;        }

        private async Task<Option<Table, Error>> CheckIfTableIsNotTaken(int tableNumber, Guid waiterToAssignToId)
        {
            // TODO: This is duplicated with the one in the OpenTabHandler
            var table = await DbContext
                .Tables
                .FirstOrDefaultAsync(t => t.Number == tableNumber);

            return table
                .SomeNotNull(Error.NotFound($"No table with number {tableNumber} was found."))
                .Filter(t => t.WaiterId != waiterToAssignToId, Error.Conflict($"Waiter {waiterToAssignToId} is already assigned to this table."));
        }
    }
}
