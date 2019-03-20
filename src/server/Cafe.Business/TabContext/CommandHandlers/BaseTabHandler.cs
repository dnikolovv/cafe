using AutoMapper;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using Optional;
using Optional.Async;
using System;
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
            DocumentSession.LoadAsync<Tab>(id, cancellationToken);

        protected Task<Option<Tab, Error>> GetTabIfExists(Guid id, CancellationToken cancellationToken) =>
            GetTabFromStore(id, cancellationToken)
                .SomeNotNull<Tab, Error>(Error.NotFound($"No tab with id {id} was found."));

        protected Task<Option<Tab, Error>> GetTabIfNotClosed(Guid id, CancellationToken cancellationToken) =>
            GetTabIfExists(id, cancellationToken)
                .FilterAsync(async tab => tab.IsOpen, Error.Validation($"Tab {id} is closed."));

        protected Task<Option<Tab, Error>> TabShouldNotExist(Guid id, CancellationToken cancellationToken) =>
            GetTabFromStore(id, cancellationToken)
                .SomeWhen<Tab, Error>(t => t == null, Error.Conflict($"Tab {id} already exists."))
                .MapAsync(async _ => new Tab(id));
    }
}
