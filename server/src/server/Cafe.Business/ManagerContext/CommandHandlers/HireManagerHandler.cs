using AutoMapper;
using Cafe.Core.ManagerContext.Commands;
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

namespace Cafe.Business.ManagerContext.CommandHandlers
{
    public class HireManagerHandler : BaseHandler<HireManager>
    {
        private readonly IManagerRepository _managerRepository;

        public HireManagerHandler(
            IValidator<HireManager> validator,
            IEventBus eventBus,
            IMapper mapper,
            IManagerRepository managerRepository)
            : base(validator, eventBus, mapper)
        {
            _managerRepository = managerRepository;
        }

        public override Task<Option<Unit, Error>> Handle(HireManager command) =>
            ManagerShouldntExist(command.Id).MapAsync(__ =>
            PersistManager(command));

        private async Task<Option<Unit, Error>> ManagerShouldntExist(Guid managerId) =>
            (await _managerRepository
                .Get(managerId))
                .SomeWhen(m => !m.HasValue, Error.Conflict($"Manager {managerId} already exists."))
                .Map(_ => Unit.Value);

        private Task<Unit> PersistManager(HireManager command)
        {
            var manager = Mapper.Map<Manager>(command);
            return _managerRepository.Add(manager);
        }
    }
}