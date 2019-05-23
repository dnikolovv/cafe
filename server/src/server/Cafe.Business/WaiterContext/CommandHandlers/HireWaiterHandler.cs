using AutoMapper;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using FluentValidation;
using MediatR;
using Optional;
using Optional.Async;
using System;
using System.Threading.Tasks;

namespace Cafe.Business.WaiterContext.CommandHandlers
{
    public class HireWaiterHandler : BaseHandler<HireWaiter>
    {
        private readonly IWaiterRepository _waiterRepository;

        public HireWaiterHandler(
            IValidator<HireWaiter> validator,
            IEventBus eventBus,
            IMapper mapper,
            IWaiterRepository waiterRepository)
            : base(validator, eventBus, mapper)
        {
            _waiterRepository = waiterRepository;
        }

        public override Task<Option<Unit, Error>> Handle(HireWaiter command) =>
            WaiterShouldntExist(command.Id).FlatMapAsync(_ =>
            PersistWaiter(command).MapAsync(__ =>
            PublishEvents(command.Id, new WaiterHired { Waiter = Mapper.Map<WaiterView>(command) })));

        private async Task<Option<Unit, Error>> WaiterShouldntExist(Guid waiterId) =>
            (await _waiterRepository.Get(waiterId))
                .SomeWhen(w => !w.HasValue, Error.Conflict($"Waiter {waiterId} already exists."))
                .Map(_ => Unit.Value);

        private async Task<Option<Unit, Error>> PersistWaiter(HireWaiter command)
        {
            var waiter = Mapper.Map<Waiter>(command);

            await _waiterRepository.Add(waiter);

            // Returning optional so we can chain
            return Unit.Value.Some<Unit, Error>();
        }
    }
}
