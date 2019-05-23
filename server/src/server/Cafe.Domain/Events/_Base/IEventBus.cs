using MediatR;
using System;
using System.Threading.Tasks;

namespace Cafe.Domain.Events
{
    public interface IEventBus
    {
        Task<Unit> Publish<TEvent>(Guid streamId, params TEvent[] events) where TEvent : IEvent;
    }
}
