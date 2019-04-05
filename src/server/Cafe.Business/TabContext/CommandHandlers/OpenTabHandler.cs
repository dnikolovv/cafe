using AutoMapper;
using Cafe.Core;
using Cafe.Core.TabContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IDocumentSession = Marten.IDocumentSession;

namespace Cafe.Business.TabContext.CommandHandlers
{
    public class OpenTabHandler : BaseTabHandler<OpenTab>, ICommandHandler<OpenTab>
    {
        public OpenTabHandler(
            IValidator<OpenTab> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper,
            IMenuItemsService menuItemsService)
            : base(validator, dbContext, documentSession, eventBus, mapper, menuItemsService)
        {
        }

        public Task<Option<Unit, Error>> Handle(OpenTab command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            TabShouldNotExist(command.Id, cancellationToken).FlatMapAsync(tab =>
            TableShouldNotBeTaken(command.TableNumber).FlatMapAsync(tableNumber =>
            TheTableShouldHaveAWaiterAssigned(tableNumber).MapAsync(waiter =>
            PublishEvents(tab.Id, tab.OpenTab(command.CustomerName, waiter.ShortName, command.TableNumber))))));

        private async Task<Option<int, Error>> TableShouldNotBeTaken(int tableNumber)
        {
            // TODO: Explicitly using the Marten AnyAsync due to conflicts with EF
            // which result in runtime errors
            // This can be fixed by using repositories/some other entities that encapsulate the data fetching
            var thereIsATabOnTable = await Marten
                .QueryableExtensions
                .AnyAsync(Session
                    .Query<TabView>()
                    .Where(t => t.IsOpen && t.TableNumber == tableNumber));

            return thereIsATabOnTable
                .SomeWhen(isTaken => !isTaken, Error.Conflict($"Table {tableNumber} is already taken."))
                .Map(_ => tableNumber);
        }

        private async Task<Option<Waiter, Error>> TheTableShouldHaveAWaiterAssigned(int tableNumber)
        {
            var table = await DbContext
                .Tables
                .Include(t => t.Waiter)
                .FirstOrDefaultAsync(t => t.Number == tableNumber);

            return table
                .SomeNotNull(Error.NotFound($"No table with number {tableNumber} was found."))
                .FlatMap(t => t.Waiter
                    .SomeNotNull(Error.Validation($"Table {tableNumber} does not have a waiter assigned.")));
        }
    }
}
