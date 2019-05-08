using AutoMapper;
using Cafe.Core.TableContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.TableContext.CommandHandlers
{
    public class CallWaiterHandler : BaseHandler<CallWaiter>
    {
        public CallWaiterHandler(
            IValidator<CallWaiter> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public override Task<Option<Unit, Error>> Handle(CallWaiter command) =>
            TableShouldExist(command.TableNumber).FlatMapAsync(table =>
            TableShouldHaveAWaiterAssigned(table).MapAsync(waiter =>
            PublishEvents(table.Id, new WaiterCalled { TableNumber = table.Number, WaiterId = waiter.Id })));

        private Option<Waiter, Error> TableShouldHaveAWaiterAssigned(Table table) =>
            table
                .Waiter
                .SomeNotNull(Error.Validation($"Table {table.Number} does not have a waiter assigned."));

        private async Task<Option<Table, Error>> TableShouldExist(int tableNumber)
        {
            var table = await DbContext
                .Tables
                .Include(t => t.Waiter)
                .FirstOrDefaultAsync(t => t.Number == tableNumber);

            return table
                .SomeNotNull(Error.NotFound($"No table with number {tableNumber} was found."));
        }
    }
}
