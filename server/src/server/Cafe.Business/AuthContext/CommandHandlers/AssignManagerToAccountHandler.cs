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
    public class AssignManagerToAccountHandler : BaseAuthHandler<AssignManagerToAccount>
    {
        private readonly IManagerRepository _managerRepository;

        public AssignManagerToAccountHandler(
            IValidator<AssignManagerToAccount> validator,
            IEventBus eventBus,
            IMapper mapper,
            IUserRepository userRepository,
            IManagerRepository managerRepository)
            : base(validator, eventBus, mapper, userRepository)
        {
            _managerRepository = managerRepository;
        }

        public override Task<Option<Unit, Error>> Handle(AssignManagerToAccount command) =>
            AccountShouldExist(command.AccountId).FlatMapAsync(account =>
            ManagerShouldExist(command.ManagerId).MapAsync(manager =>
            ReplaceClaim(account, AuthConstants.ClaimTypes.ManagerId, manager.Id.ToString())));

        private Task<Option<Manager, Error>> ManagerShouldExist(Guid managerId) =>
            _managerRepository
                .Get(managerId)
                .WithException(Error.NotFound($"No manager with id {managerId} was found."));
    }
}
