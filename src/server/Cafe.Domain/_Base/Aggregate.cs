using Cafe.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cafe.Domain
{
    public class Aggregate
    {
        public Guid Id { get; protected set; }

        public Queue<IEvent> PendingEvents { get; private set; } = new Queue<IEvent>();
    }
}
