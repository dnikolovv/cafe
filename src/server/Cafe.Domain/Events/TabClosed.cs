using System;

namespace Cafe.Domain.Events
{
    public class TabClosed
    {
        public Guid TabId { get; set; }
        public int TableNumber { get; set; }
    }
}
