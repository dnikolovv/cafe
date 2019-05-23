using AutoMapper;
using Cafe.Core.CashierContext.Commands;
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

namespace Cafe.Business.CashierContext.CommandHandlers
{
    public class HireCashierHandler : BaseHandler<HireCashier>
    {
        private readonly ICashierRepository _cashierRepository;

        public HireCashierHandler(
            IValidator<HireCashier> validator,
            IEventBus eventBus,
            IMapper mapper,
            ICashierRepository cashierRepository)
            : base(validator, eventBus, mapper)
        {
            _cashierRepository = cashierRepository;
        }

        public override Task<Option<Unit, Error>> Handle(HireCashier command) =>
            CashierShouldntExist(command.Id).MapAsync(__ =>
            PersistCashier(command));

        private async Task<Option<Unit, Error>> CashierShouldntExist(Guid cashierId) =>
            (await _cashierRepository
                .Get(cashierId))
                .SomeWhen(c => !c.HasValue, Error.Conflict($"Cashier {cashierId} already exists."))
                .Map(_ => Unit.Value);

        private Task<Unit> PersistCashier(HireCashier command)
        {
            var cashier = Mapper.Map<Cashier>(command);
            return _cashierRepository.Add(cashier);
        }
    }
}