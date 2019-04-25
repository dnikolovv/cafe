using Cafe.Domain.Events;
using MediatR;
using System.Threading.Tasks;

namespace Cafe.Business
{
    public class EventBus : IEventBus
    {
        private readonly IMediator _mediator;

        public EventBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish<TEvent>(params TEvent[] events)
            where TEvent : IEvent
        {
            foreach (var @event in events)
            {
                await _mediator.Publish(@event);
            }
        }
    }
}
