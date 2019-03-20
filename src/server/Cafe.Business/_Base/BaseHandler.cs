using Cafe.Domain;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using MediatR;
using Optional;
using Optional.Async;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.Business
{
    public abstract class BaseHandler<TCommand>
    {
        public BaseHandler(
            IValidator<TCommand> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus)
        {
            Validator = validator ??
                throw new InvalidOperationException(
                    "Tried to instantiate a command handler without a validator." +
                    "Did you forget to add one?");

            DbContext = dbContext;
            DocumentSession = documentSession;
            EventBus = eventBus;
        }

        protected IValidator<TCommand> Validator { get; }

        protected ApplicationDbContext DbContext { get; }

        protected IDocumentSession DocumentSession { get; }

        protected IEventBus EventBus { get; }

        protected Option<TCommand, Error> ValidateCommand(TCommand command)
        {
            var validationResult = Validator.Validate(command);

            return validationResult
                .SomeWhen(
                    r => r.IsValid,
                    r => Error.Validation(r.Errors.Select(e => e.ErrorMessage)))

                // If the validation result is successfull, disregard it and simply return the command
                .Map(_ => command);
        }

        protected async Task<Unit> PublishEvents(Guid tabId, params IEvent[] events)
        {
            DocumentSession.Events.Append(tabId, events);
            await DocumentSession.SaveChangesAsync();
            await EventBus.Publish(events);

            return Unit.Value;
        }
    }
}
