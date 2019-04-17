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
using System.Collections.Generic;
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.OrderContext.CommandHandlers
{
    public class OrderToGoHandler : BaseHandler<OrderToGo>
    {
        private readonly IMenuItemsService _menuItemsService;

        public OrderToGoHandler(
            IValidator<OrderToGo> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper,
            IMenuItemsService menuItemsService)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
            _menuItemsService = menuItemsService;
        }

        public override Task<Option<Unit, Error>> Handle(OrderToGo command) =>
            CheckIfOrderIsNotExisting(command).FlatMapAsync(order =>
            MenuItemsShouldExist(command.ItemNumbers).MapAsync(items =>
            PersistOrder(order, items)));

        private async Task<Option<ToGoOrder, Error>> CheckIfOrderIsNotExisting(OrderToGo command)
        {
            var orderInDb = await DbContext
                .ToGoOrders
                .FirstOrDefaultAsync(o => o.Id == command.Id);

            return command
                .SomeWhen(_ => orderInDb == null, Error.Conflict($"Order {command.Id} already exists."))
                .Map(Mapper.Map<ToGoOrder>);
        }

        private Task<Option<IList<MenuItem>, Error>> MenuItemsShouldExist(IList<int> numbers) =>
            _menuItemsService.ItemsShouldExist(numbers);

        private async Task<Unit> PersistOrder(ToGoOrder order, IList<MenuItem> orderedItems)
        {
            order.OrderedItems = orderedItems;
            DbContext.ToGoOrders.Add(order);
            await DbContext.SaveChangesAsync();
            return Unit.Value;
        }
    }
}