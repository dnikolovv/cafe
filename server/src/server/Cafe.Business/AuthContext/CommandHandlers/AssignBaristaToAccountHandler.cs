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
    public class AssignBaristaToAccountHandler : BaseAuthHandler<AssignBaristaToAccount>
    {
        private readonly IBaristaRepository _baristaRepository;

        public AssignBaristaToAccountHandler(
            IValidator<AssignBaristaToAccount> validator,
            IEventBus eventBus,
            IMapper mapper,
            IUserRepository userRepository,
            IBaristaRepository baristaRepository)
            : base(validator, eventBus, mapper, userRepository)
        {
            _baristaRepository = baristaRepository;
        }

        public override Task<Option<Unit, Error>> Handle(AssignBaristaToAccount command) =>
            AccountShouldExist(command.AccountId).FlatMapAsync(account =>
            BaristaShouldExist(command.BaristaId).MapAsync(barista =>
            ReplaceClaim(account, AuthConstants.ClaimTypes.BaristaId, barista.Id.ToString())));

        private Task<Option<Barista, Error>> BaristaShouldExist(Guid baristaId) =>
            _baristaRepository
                .Get(baristaId)
                .WithException(Error.NotFound($"No barista with id {baristaId} was found."));
    }
}
