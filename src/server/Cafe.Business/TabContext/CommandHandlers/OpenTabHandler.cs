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
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public Task<Option<Unit, Error>> Handle(OpenTab command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            TabShouldNotExist(command.Id, cancellationToken).FlatMapAsync(tab =>
            GetWaiterForTableIfAvailable(command.TableNumber).MapAsync(waiter =>
            PublishEvents(tab.Id, tab.OpenTab(command.CustomerName, waiter.ShortName, command.TableNumber)))));

        private async Task<Option<Waiter, Error>> GetWaiterForTableIfAvailable(int tableNumber)
        {
            var table = await DbContext
                .Tables
                .Include(t => t.Waiter)
                .FirstOrDefaultAsync(t => t.Number == tableNumber);

            return table
                .SomeNotNull(Error.NotFound($"No table with number {tableNumber} was found."))
                .Map(t => t.Waiter);
        }
    }
}
