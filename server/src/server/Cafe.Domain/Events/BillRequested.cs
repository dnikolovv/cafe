using System;

namespace Cafe.Domain.Events
{
    public class BillRequested : IEvent
    {
        public int TableNumber { get; set; }

        public Guid WaiterId { get; set; }

        public Guid TabId { get; set; }
    }
}
