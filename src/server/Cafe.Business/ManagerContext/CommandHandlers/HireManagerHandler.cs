using AutoMapper;
using Cafe.Core;
using Cafe.Core.ManagerContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using MediatR;
using Optional;
using Optional.Async;
using System.Threading;
using System.Threading.Tasks;

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
            ValidateCommand(command).MapAsync(_ =>
            PersistManager(command));

        private async Task<Unit> PersistManager(HireManager command)
        {
            var manager = Mapper.Map<Manager>(command);

            DbContext.Managers.Add(manager);

            await DbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
