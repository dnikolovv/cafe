using AutoMapper;
using Cafe.Core.AuthContext;
using Cafe.Core.AuthContext.Commands;
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

namespace Cafe.Business.AuthContext.CommandHandlers
{
    public class AssignCashierToAccountHandler : BaseAuthHandler<AssignCashierToAccount>
    {
        private readonly ICashierRepository _cashierRepository;

        public AssignCashierToAccountHandler(
            IValidator<AssignCashierToAccount> validator,
            IEventBus eventBus,
            IMapper mapper,
            IUserRepository userRepository,
            ICashierRepository cashierRepository)
            : base(validator, eventBus, mapper, userRepository)
        {
            _cashierRepository = cashierRepository;
        }

        public override Task<Option<Unit, Error>> Handle(AssignCashierToAccount command) =>
            AccountShouldExist(command.AccountId).FlatMapAsync(account =>
            CashierShouldExist(command.CashierId).MapAsync(cashier =>
            ReplaceClaim(account, AuthConstants.ClaimTypes.CashierId, cashier.Id.ToString())));

        private Task<Option<Cashier, Error>> CashierShouldExist(Guid cashierId) =>
            _cashierRepository
                .Get(cashierId)
                .WithException(Error.NotFound($"No cashier with id {cashierId} was found."));
    }
}
