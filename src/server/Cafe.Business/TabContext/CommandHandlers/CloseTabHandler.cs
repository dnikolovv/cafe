using AutoMapper;
using Cafe.Core;
using Cafe.Core.TabContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using MediatR;
using Optional;
using Optional.Async;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.TabContext.CommandHandlers
{
    public class CloseTabHandler : BaseTabHandler<CloseTab>, ICommandHandler<CloseTab>
    {
        public CloseTabHandler(
            IValidator<CloseTab> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public Task<Option<Unit, Error>> Handle(CloseTab command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(_ =>
            TabShouldNotBeClosed(command.TabId, cancellationToken).
            FilterAsync(async tab => command.AmountPaid >= tab.ServedItemsValue, Error.Validation("You cannot pay less than the bill amount.")).MapAsync(tab =>
            PublishEvents(tab.Id, tab.CloseTab(command.AmountPaid))));
    }
}
