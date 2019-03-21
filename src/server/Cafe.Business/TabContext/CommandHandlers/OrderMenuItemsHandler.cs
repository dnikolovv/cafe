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
    public class OrderMenuItemsHandler : BaseTabHandler<OrderMenuItems>, ICommandHandler<OrderMenuItems>
    {
        public OrderMenuItemsHandler(
            IValidator<OrderMenuItems> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public Task<Option<Unit, Error>> Handle(OrderMenuItems command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            TabShouldNotBeClosed(command.TabId, cancellationToken).FlatMapAsync(tab =>
            GetMenuItemsIfTheyExist(command.ItemNumbers).MapAsync(items =>
            PublishEvents(command.TabId, tab.OrderMenuItems(items)))));
    }
}
