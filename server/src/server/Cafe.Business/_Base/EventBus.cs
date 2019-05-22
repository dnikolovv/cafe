using Cafe.Domain.Events;
using Marten;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Cafe.Business
{
    public class EventBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly IDocumentSession _session;

        public EventBus(IMediator mediator, IDocumentSession session)
        {
            _mediator = mediator;
            _session = session;
        }

        public async Task<Unit> Publish<TEvent>(Guid streamId, params TEvent[] events)
            where TEvent : IEvent
        {
            _session.Events.Append(streamId, events);
            await _session.SaveChangesAsync();

            foreach (var @event in events)
            {
                await _mediator.Publish(@event);
            }

            return Unit.Value;
        }
    }
}
