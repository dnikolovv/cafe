using Cafe.Domain.Events;
using System;
using System.Collections.Generic;

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
        public int TableNumber { get; set; }
        public IList<MenuItem> OrderedMenuItems { get; set; } = new List<MenuItem>();

        public TabOpened OpenTab(string customerName, string waiterName, int tableNumber) =>
            new TabOpened
            {
                TabId = Id,
                CustomerName = customerName,
                WaiterName = waiterName,
                TableNumber = tableNumber
            };

        public MenuItemsOrdered OrderMenuItems(IList<MenuItem> items) =>
            new MenuItemsOrdered
            {
                TabId = Id,
                MenuItems = items
            };

        public void Apply(TabOpened @event)
        {
            IsOpen = true;
            CustomerName = @event.CustomerName;
            WaiterName = @event.WaiterName;
            TableNumber = @event.TableNumber;
        }

        public void Apply(MenuItemsOrdered @event)
        {
            foreach (var item in @event.MenuItems)
            {
                OrderedMenuItems.Add(item);
            }
        }
    }
}
