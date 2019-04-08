using AutoMapper;
using Cafe.Core;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using MediatR;
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
            IValidator<TCommand> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper,
            IMenuItemsService menuItemsService)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
            MenuItemsService = menuItemsService;
        }

        protected IMenuItemsService MenuItemsService { get; }

        protected Task<Tab> GetTabFromStore(Guid id) =>
                    Session.LoadAsync<Tab>(id);

        protected Task<Option<Tab, Error>> GetTabIfExists(Guid id) =>
            GetTabFromStore(id)
                .SomeNotNull<Tab, Error>(Error.NotFound($"No tab with id {id} was found."));

        protected Task<Option<IList<MenuItem>, Error>> MenuItemsShouldExist(IList<int> menuItemNumbers) =>
            // Wrapping to improve readability
            MenuItemsService.ItemsShouldExist(menuItemNumbers);

        protected Task<Option<Tab, Error>> TabShouldExist(Guid id) =>
            GetTabFromStore(id)
                .SomeWhen<Tab, Error>(t => t != null, Error.Conflict($"Tab {id} does not exist."))
                .MapAsync(async _ => new Tab(id));

        protected Task<Option<Tab, Error>> TabShouldNotBeClosed(Guid id) =>
                            GetTabIfExists(id)
                .FilterAsync(async tab => tab.IsOpen, Error.Validation($"Tab {id} is closed."));

        protected Task<Option<Tab, Error>> TabShouldNotExist(Guid id) =>
            GetTabFromStore(id)
                .SomeWhen<Tab, Error>(t => t == null, Error.Conflict($"Tab {id} already exists."))
                .MapAsync(async _ => new Tab(id));
    }
}