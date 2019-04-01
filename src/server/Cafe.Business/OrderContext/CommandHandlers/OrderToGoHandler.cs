using AutoMapper;
using Cafe.Core;
using Cafe.Core.OrderContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using Optional.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.OrderContext.CommandHandlers
{
    public class OrderToGoHandler : BaseHandler<OrderToGo>, ICommandHandler<OrderToGo>
    {
        public OrderToGoHandler(
            IValidator<OrderToGo> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public Task<Option<Unit, Error>> Handle(OrderToGo command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            CheckIfOrderIsNotExisting(command).FlatMapAsync(order =>
            GetMenuItemsIfTheyExist(command.ItemNumbers).MapAsync(items =>
            PersistOrder(order, items))));

        private async Task<Unit> PersistOrder(Order order, IList<MenuItem> orderedItems)
        {
            order.Items = orderedItems;
            DbContext.Orders.Add(order);
            await DbContext.SaveChangesAsync();
            return Unit.Value;
        }

        private async Task<Option<Order, Error>> CheckIfOrderIsNotExisting(OrderToGo command)
        {
            var orderInDb = await DbContext
                .Orders
                .FirstOrDefaultAsync(o => o.Id == command.Id);

            return orderInDb
                .SomeWhen(o => o == null, Error.Conflict($"Order {command.Id} already exists."))
                .Map(_ => Mapper.Map<Order>(command));
        }

        /// <summary>
        /// TODO: This is duplicated in BaseTabHandler.cs
        /// </summary>
        private async Task<Option<IList<MenuItem>, Error>> GetMenuItemsIfTheyExist(IList<int> menuItemNumbers)
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
