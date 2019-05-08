using System;

namespace Cafe.Domain.Events
{
    public class WaiterCalled : IEvent
    {
        public int TableNumber { get; set; }

        public Guid WaiterId { get; set; }
    }
}
