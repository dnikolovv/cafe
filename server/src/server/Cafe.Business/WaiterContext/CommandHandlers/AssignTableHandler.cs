using AutoMapper;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Repositories;
using FluentValidation;
using MediatR;
using Optional;
using Optional.Async;
using System;
using System.Threading.Tasks;

namespace Cafe.Business.WaiterContext.CommandHandlers
{
    public class AssignTableHandler : BaseHandler<AssignTable>
    {
        private readonly IWaiterRepository _waiterRepository;
        private readonly ITableRepository _tableRepository;

        public AssignTableHandler(
            IValidator<AssignTable> validator,
            IEventBus eventBus,
            IMapper mapper,
            IWaiterRepository waiterRepository,
            ITableRepository tableRepository)
            : base(validator, eventBus, mapper)
        {
            _waiterRepository = waiterRepository;
            _tableRepository = tableRepository;
        }

        public override Task<Option<Unit, Error>> Handle(AssignTable command) =>
            CheckIfTableExists(command.TableNumber).
            FilterAsync(async t => t.WaiterId != command.WaiterId, Error.Conflict($"Waiter {command.WaiterId} is already assigned to this table.")).FlatMapAsync(table =>
            CheckIfWaiterExists(command.WaiterId).MapAsync(waiter =>
            AssignTable(table, waiter.Id)));

        private Task<Unit> AssignTable(Table table, Guid waiterToAssignToId)
        {
            table.WaiterId = waiterToAssignToId;
            return _tableRepository.Update(table);
        }

        private Task<Option<Waiter, Error>> CheckIfWaiterExists(Guid waiterId) =>
            _waiterRepository
                .Get(waiterId)
                .WithException(Error.NotFound($"No waiter with an id of {waiterId} was found."));

        private Task<Option<Table, Error>> CheckIfTableExists(int tableNumber) =>
            _tableRepository
                .GetByNumber(tableNumber)
                .WithException(Error.NotFound($"No table with number {tableNumber} was found."));
    }
}
