using AutoMapper;
using Cafe.Core;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Repositories;
using FluentValidation;
using Optional;
using Optional.Async;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Business.TabContext.CommandHandlers
{
    public abstract class BaseTabHandler<TCommand> : BaseHandler<TCommand>
        where TCommand : ICommand
    {
        public BaseTabHandler(
            ITabRepository tabRepository,
            IMenuItemsService menuItemsService,
            IValidator<TCommand> validator,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, eventBus, mapper)
        {
            TabRepository = tabRepository;
            MenuItemsService = menuItemsService;
        }

        protected IMenuItemsService MenuItemsService { get; }
        protected ITabRepository TabRepository { get; }

        protected async Task<Option<Tab, Error>> GetTabIfExists(Guid id) =>
            (await TabRepository.Get(id))
                .WithException(Error.NotFound($"No tab with id {id} was found."));

        protected Task<Option<IList<MenuItem>, Error>> MenuItemsShouldExist(IList<int> menuItemNumbers) =>
            MenuItemsService.ItemsShouldExist(menuItemNumbers);

        protected Task<Option<Tab, Error>> TabShouldNotBeClosed(Guid id) =>
            GetTabIfExists(id)
                .FilterAsync(async tab => tab.IsOpen, Error.Validation($"Tab {id} is closed."));

        protected async Task<Option<Tab, Error>> TabShouldNotExist(Guid id) =>
            (await TabRepository.Get(id))
                .SomeWhen(t => !t.HasValue, Error.Conflict($"Tab {id} already exists."))
                .Map(_ => new Tab(id));
    }
}