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
using Optional;
using Optional.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.TabContext.CommandHandlers
{
    public class RejectMenuItemsHandler : BaseTabHandler<RejectMenuItems>, ICommandHandler<RejectMenuItems>
    {
        public RejectMenuItemsHandler(
            IValidator<RejectMenuItems> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public Task<Option<Unit, Error>> Handle(RejectMenuItems command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            TabShouldNotBeClosed(command.TabId, cancellationToken).FlatMapAsync(tab =>
            GetMenuItemsIfTheyExist(command.ItemNumbers).FlatMapAsync(items =>
            TheItemsShouldHaveBeenOrdered(tab, command.ItemNumbers).MapAsync(__ =>
            PublishEvents(tab.Id, tab.RejectMenuItems(items))))));

        private Option<Unit, Error> TheItemsShouldHaveBeenOrdered(Tab tab, IList<int> itemNumbers)
        {
            var allTabItemNumbers = tab
                .OutstandingMenuItems
                .Concat(tab.ServedMenuItems)
                .ToLookup(i => i.Number);

            var unorderedItems = itemNumbers
                .Where(n => !allTabItemNumbers.Contains(n))
                .ToArray();

            return unorderedItems
                .SomeWhen(
                    items => items.Length == 0,
                    Error.Validation($"Attempted to reject menu items {string.Join(", ", unorderedItems)} which haven't been ordered."))
                .Map(_ => Unit.Value);
        }
    }
}
