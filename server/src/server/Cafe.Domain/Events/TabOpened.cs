using System;

namespace Cafe.Domain.Events
{
    public class TabOpened : IEvent
    {
        public Guid TabId { get; set; }

        public string CustomerName { get; set; }

        public string WaiterName { get; set; }

        public int TableNumber { get; set; }
    }
}
