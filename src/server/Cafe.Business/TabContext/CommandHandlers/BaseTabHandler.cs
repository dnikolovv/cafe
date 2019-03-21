using AutoMapper;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using Optional.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.TabContext.CommandHandlers
{
    public class BaseTabHandler<TCommand> : BaseHandler<TCommand>
    {
        public BaseTabHandler(
            IValidator<TCommand> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        protected Task<Tab> GetTabFromStore(Guid id, CancellationToken cancellationToken) =>
            Session.LoadAsync<Tab>(id, cancellationToken);

        protected Task<Option<Tab, Error>> GetTabIfExists(Guid id, CancellationToken cancellationToken) =>
            GetTabFromStore(id, cancellationToken)
                .SomeNotNull<Tab, Error>(Error.NotFound($"No tab with id {id} was found."));

        protected Task<Option<Tab, Error>> TabShouldNotBeClosed(Guid id, CancellationToken cancellationToken) =>
            GetTabIfExists(id, cancellationToken)
                .FilterAsync(async tab => tab.IsOpen, Error.Validation($"Tab {id} is closed."));

        protected Task<Option<Tab, Error>> TabShouldNotExist(Guid id, CancellationToken cancellationToken) =>
            GetTabFromStore(id, cancellationToken)
                .SomeWhen<Tab, Error>(t => t == null, Error.Conflict($"Tab {id} already exists."))
                .MapAsync(async _ => new Tab(id));

        protected Task<Option<Tab, Error>> TabShouldExist(Guid id, CancellationToken cancellationToken) =>
            GetTabFromStore(id, cancellationToken)
                .SomeWhen<Tab, Error>(t => t != null, Error.Conflict($"Tab {id} does not exist."))
                .MapAsync(async _ => new Tab(id));

        protected async Task<Option<IList<MenuItem>, Error>> GetMenuItemsIfTheyExist(IList<int> menuItemNumbers)
        {
            var allItemsInStock = (await EntityFrameworkQueryableExtensions.ToListAsync(DbContext.MenuItems))
                .ToDictionary(x => x.Number);

            var itemsToServeResult = menuItemNumbers
                .Select(menuNumber => allItemsInStock
                    .GetValueOrNone(menuNumber)
                    .WithException(Error.NotFound($"Menu item with number {menuNumber} was not found.")))
                .ToList();

            var errors = itemsToServeResult
                .Exceptions()
                .SelectMany(e => e.Messages);

            return errors.Any() ?
                Option.None<IList<MenuItem>, Error>(Error.NotFound(errors)) :
                itemsToServeResult.Values().ToList().Some<IList<MenuItem>, Error>();
        }
    }
}
