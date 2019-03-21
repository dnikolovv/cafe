using AutoMapper;
using Cafe.Core;
using Cafe.Core.TabContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using MediatR;
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
            GetItemsIfTheyExist(command.ItemNumbers).MapAsync(items =>
            PublishEvents(command.TabId, tab.OrderMenuItems(items)))));

        private async Task<Option<IList<MenuItem>, Error>> GetItemsIfTheyExist(IList<int> menuItemNumbers)
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
