using AutoMapper;
using Cafe.Core.MenuContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.Business.MenuContext.CommandHandlers
{
    public class AddMenuItemsHandler : BaseHandler<AddMenuItems>
    {
        public AddMenuItemsHandler(
            IValidator<AddMenuItems> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public override Task<Option<Unit, Error>> Handle(AddMenuItems command) =>
            CheckIfNumbersAreNotConflicting(command.MenuItems.Select(i => i.Number)).MapAsync(__ =>
            PersistMenuItems(command.MenuItems));

        private async Task<Option<Unit, Error>> CheckIfNumbersAreNotConflicting(IEnumerable<int> itemNumbersToCheck)
        {
            var numbersLookup = itemNumbersToCheck
                .Distinct()
                .ToArray();

            var conflictingNumbers = await DbContext
                .MenuItems
                .Where(i => numbersLookup.Contains(i.Number))
                .Select(i => i.Number)
                .ToArrayAsync();

            return conflictingNumbers
                .SomeWhen(
                    numbers => numbers.Length == 0,
                    Error.Conflict($"Item numbers {string.Join(", ", conflictingNumbers)} are already taken."))
                .Map(_ => Unit.Value);
        }

        private async Task<Unit> PersistMenuItems(IEnumerable<MenuItemView> items)
        {
            var itemsToAdd = Mapper.Map<MenuItem[]>(items);

            DbContext.MenuItems.AddRange(itemsToAdd);
            await DbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
