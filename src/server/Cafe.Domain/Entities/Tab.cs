using Cafe.Domain.Events;
using System;

namespace Cafe.Domain.Entities
{
    public class Tab : IAggregate
    {
        public Tab()
        {
        }

        public Tab(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string WaiterName { get; set; }
        public bool IsOpen { get; set; }
        public decimal ServedItemsValue { get; set; }

        public TabOpened OpenTab(string customerName, string waiterName, int tableNumber) =>
            new TabOpened
            {
                TabId = Id,
                CustomerName = customerName,
                WaiterName = waiterName,
                TableNumber = tableNumber
            };
    }
}
