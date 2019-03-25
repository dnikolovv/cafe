using AutoMapper;
using Cafe.Core;
using Cafe.Core.TabContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using MediatR;
using Optional;
using Optional.Async;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.TabContext.CommandHandlers
{
    public class ServeMenuItemsHandler : BaseTabHandler<ServeMenuItems>, ICommandHandler<ServeMenuItems>
    {
        public ServeMenuItemsHandler(
            IValidator<ServeMenuItems> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public Task<Option<Unit, Error>> Handle(ServeMenuItems command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            TabShouldNotBeClosed(command.TabId, cancellationToken).FlatMapAsync(tab =>
            GetMenuItemsIfTheyExist(command.ItemNumbers).MapAsync(items =>
            PublishEvents(tab.Id, tab.ServeMenuItems(items)))));
    }
}
