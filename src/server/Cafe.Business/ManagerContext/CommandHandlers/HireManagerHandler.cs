using AutoMapper;
using Cafe.Core;
using Cafe.Core.ManagerContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using System;
using System.Threading;
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.ManagerContext.CommandHandlers
{
    public class HireManagerHandler : BaseHandler<HireManager>, ICommandHandler<HireManager>
    {
        public HireManagerHandler(
            IValidator<HireManager> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public Task<Option<Unit, Error>> Handle(HireManager command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            ManagerShouldntExist(command.Id).MapAsync(__ =>
            PersistManager(command)));

        private async Task<Option<Unit, Error>> ManagerShouldntExist(Guid managerId) =>
            (await DbContext
                .Managers
                .FirstOrDefaultAsync(m => m.Id == managerId))
                .SomeWhen(m => m == null, Error.Conflict($"Manager {managerId} already exists."))
                .Map(_ => Unit.Value);

        private async Task<Unit> PersistManager(HireManager command)
        {
            var manager = Mapper.Map<Manager>(command);

            DbContext.Managers.Add(manager);

            await DbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}