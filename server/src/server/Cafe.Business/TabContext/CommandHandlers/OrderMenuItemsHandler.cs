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
using System.Threading.Tasks;

namespace Cafe.Business.TabContext.CommandHandlers
{
    public class OrderMenuItemsHandler : BaseTabHandler<OrderMenuItems>
    {
        public OrderMenuItemsHandler(
            IValidator<OrderMenuItems> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper,
            IMenuItemsService menuItemsService)
            : base(validator, dbContext, documentSession, eventBus, mapper, menuItemsService)
        {
        }

        public override Task<Option<Unit, Error>> Handle(OrderMenuItems command) =>
            TabShouldNotBeClosed(command.TabId).FlatMapAsync(tab =>
            MenuItemsShouldExist(command.ItemNumbers).MapAsync(items =>
            PublishEvents(command.TabId, tab.OrderMenuItems(items))));
    }
}
