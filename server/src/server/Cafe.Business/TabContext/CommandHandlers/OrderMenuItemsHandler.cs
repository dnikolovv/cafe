using AutoMapper;
using Cafe.Core;
using Cafe.Core.TabContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Events;
using Cafe.Domain.Repositories;
using FluentValidation;
using MediatR;
using Optional;
using Optional.Async;
using System.Threading.Tasks;

namespace Cafe.Business.TabContext.CommandHandlers
{
    public class OrderMenuItemsHandler : BaseTabHandler<OrderMenuItems>
    {
        public OrderMenuItemsHandler(
            ITabRepository tabRepository,
            IMenuItemsService menuItemsService,
            IValidator<OrderMenuItems> validator,
            IEventBus eventBus,
            IMapper mapper)
            : base(tabRepository, menuItemsService, validator, eventBus, mapper)
        {
        }

        public override Task<Option<Unit, Error>> Handle(OrderMenuItems command) =>
            TabShouldNotBeClosed(command.TabId).FlatMapAsync(tab =>
            MenuItemsShouldExist(command.ItemNumbers).MapAsync(items =>
            PublishEvents(command.TabId, tab.OrderMenuItems(items))));
    }
}
