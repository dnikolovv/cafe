using Cafe.Core.Tab.Commands;
using Cafe.Domain;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using MediatR;
using Optional;
using Optional.Async;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.TabHandlers.Commands
{
    public class OpenTabHandler : BaseTabHandler<OpenTab>
    {
        public OpenTabHandler(
            IValidator<OpenTab> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus)
            : base(validator, dbContext, documentSession, eventBus)
        {
        }

        public Task<Option<Unit, Error>> Handle(OpenTab command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            TabShouldNotExist(command.Id, cancellationToken).FlatMapAsync(tab =>
            TableShouldNotBeTaken(command.TableNumber).MapAsync(waiter =>
            PublishEvents(tab.Id, tab.OpenTab(command.ClientName, waiter.ShortName, command.TableNumber)))));

        // Just to make it compile
        private Option<Waiter, Error> TableShouldNotBeTaken(int tableNumber) =>
            default;

        public class Waiter { public string ShortName { get; set; } }
    }
}
