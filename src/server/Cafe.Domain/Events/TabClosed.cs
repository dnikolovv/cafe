using System;

namespace Cafe.Domain.Events
{
    public class TabClosed : IEvent
    {
        public Guid TabId { get; set; }
        public int TableNumber { get; set; }
    }
}
