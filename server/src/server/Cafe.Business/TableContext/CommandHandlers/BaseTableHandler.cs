using AutoMapper;
using Cafe.Core;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Optional;
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.TableContext.CommandHandlers
{
    public abstract class BaseTableHandler<TCommand> : BaseHandler<TCommand>
        where TCommand : ICommand
    {
        public BaseTableHandler(
            IValidator<TCommand> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        protected Option<Waiter, Error> TableShouldHaveAWaiterAssigned(Table table) =>
            table
                .Waiter
                .SomeNotNull(Error.Validation($"Table {table.Number} does not have a waiter assigned."));

        protected async Task<Option<Table, Error>> TableShouldExist(int tableNumber)
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
