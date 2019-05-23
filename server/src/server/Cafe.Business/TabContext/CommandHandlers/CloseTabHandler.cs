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
    public class CloseTabHandler : BaseTabHandler<CloseTab>
    {
        public CloseTabHandler(
            ITabRepository tabRepository,
            IMenuItemsService menuItemsService,
            IValidator<CloseTab> validator,
            IEventBus eventBus,
            IMapper mapper)
            : base(tabRepository, menuItemsService, validator, eventBus, mapper)
        {
        }

        public override Task<Option<Unit, Error>> Handle(CloseTab command) =>
            TabShouldNotBeClosed(command.TabId).
            FilterAsync(async tab => command.AmountPaid >= tab.ServedItemsValue, Error.Validation("You cannot pay less than the bill amount.")).MapAsync(tab =>
            PublishEvents(tab.Id, tab.CloseTab(command.AmountPaid)));
    }
}
