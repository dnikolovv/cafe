using Cafe.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public IList<MenuItem> OutstandingMenuItems { get; set; } = new List<MenuItem>();
        public IList<MenuItem> ServedMenuItems { get; set; } = new List<MenuItem>();

        public TabOpened OpenTab(string customerName, string waiterName, int tableNumber) =>
            new TabOpened
            {
                TabId = Id,
                CustomerName = customerName,
                WaiterName = waiterName,
                TableNumber = tableNumber
            };

        public TabClosed CloseTab(decimal amountPaid) =>
            new TabClosed
            {
                TabId = Id,
                AmountPaid = amountPaid,
                OrderValue = ServedItemsValue,
                TableNumber = TableNumber,
                TipValue = amountPaid - ServedItemsValue,
                WaiterName = WaiterName
            };

        public MenuItemsOrdered OrderMenuItems(IList<MenuItem> items) =>
            new MenuItemsOrdered
            {
                TabId = Id,
                MenuItems = items
            };

        public MenuItemsServed ServeMenuItems(IList<MenuItem> items) =>
            new MenuItemsServed
            {
                TabId = Id,
                MenuItems = items
            };

        public MenuItemsRejected RejectMenuItems(IList<MenuItem> items) =>
            new MenuItemsRejected
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

        public void Apply(TabClosed @event)
        {
            IsOpen = false;
        }

        public void Apply(MenuItemsOrdered @event)
        {
            foreach (var item in @event.MenuItems)
            {
                OutstandingMenuItems.Add(item);
            }
        }

        public void Apply(MenuItemsServed @event)
        {
            foreach (var item in @event.MenuItems)
            {
                ServedMenuItems.Add(item);
            }

            ServedItemsValue += @event.MenuItems.Sum(i => i.Price);

            foreach (var servedItem in @event.MenuItems)
            {
                var outstanding = OutstandingMenuItems
                    .FirstOrDefault(b => b.Number == servedItem.Number);

                if (outstanding != null)
                {
                    OutstandingMenuItems.Remove(outstanding);
                }
            }
        }

        public void Apply(MenuItemsRejected @event)
        {
            foreach (var rejectedItem in @event.MenuItems)
            {
                // Prioritize the oustanding over the served ones when removing items
                var outstanding = OutstandingMenuItems
                    .FirstOrDefault(b => b.Number == rejectedItem.Number);

                // TODO: Fix fugly implementation
                if (outstanding != null)
                {
                    OutstandingMenuItems.Remove(outstanding);
                }
                else
                {
                    var served = ServedMenuItems
                        .FirstOrDefault(b => b.Number == rejectedItem.Number);

                    if (served != null)
                    {
                        ServedMenuItems.Remove(served);
                    }
                }
            }
        }
    }
}
