using AutoMapper;
using Cafe.Core;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Repositories;
using FluentValidation;
using Optional;
using System.Threading.Tasks;

namespace Cafe.Business.TableContext.CommandHandlers
{
    public abstract class BaseTableHandler<TCommand> : BaseHandler<TCommand>
        where TCommand : ICommand
    {
        protected BaseTableHandler(
            IValidator<TCommand> validator,
            IEventBus eventBus,
            IMapper mapper,
            ITableRepository tableRepository)
            : base(validator, eventBus, mapper)
        {
            TableRepository = tableRepository;
        }

        protected ITableRepository TableRepository { get; }

        protected Option<Waiter, Error> TableShouldHaveAWaiterAssigned(Table table) =>
            table
                .Waiter
                .SomeNotNull(Error.Validation($"Table {table.Number} does not have a waiter assigned."));

        protected async Task<Option<Table, Error>> TableShouldExist(int tableNumber)
        {
            var table = await TableRepository
                .GetByNumber(tableNumber);

            return table
                .WithException(Error.NotFound($"No table with number {tableNumber} was found."));
        }
    }
}
