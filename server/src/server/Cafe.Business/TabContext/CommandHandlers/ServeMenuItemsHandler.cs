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
    public class ServeMenuItemsHandler : BaseTabHandler<ServeMenuItems>
    {
        public ServeMenuItemsHandler(
            ITabRepository tabRepository,
            IMenuItemsService menuItemsService,
            IValidator<ServeMenuItems> validator,
            IEventBus eventBus,
            IMapper mapper)
            : base(tabRepository, menuItemsService, validator, eventBus, mapper)
        {
        }

        public override Task<Option<Unit, Error>> Handle(ServeMenuItems command) =>
            TabShouldNotBeClosed(command.TabId).FlatMapAsync(tab =>
            MenuItemsShouldExist(command.ItemNumbers).MapAsync(items =>
            PublishEvents(tab.Id, tab.ServeMenuItems(items))));
    }
}
