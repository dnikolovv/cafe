using System;

namespace Cafe.Domain.Events
{
    public class TabClosed : IEvent
    {
        public Guid TabId { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal TipValue { get; set; }
        public decimal OrderValue { get; set; }
        public string WaiterName { get; set; }
        public int TableNumber { get; set; }
    }
}
