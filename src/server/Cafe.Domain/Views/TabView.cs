using Baseline;
using Cafe.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafe.Domain.Views
{
    public class TabView
    {
        public Guid Id { get; set; }

        public int TableNumber { get; set; }

        public string CustomerName { get; set; }

        public string WaiterName { get; set; }

        public decimal ServedItemsValue { get; set; }

        public decimal RejectedItemsValue { get; set; }

        public bool IsOpen { get; set; }

        public decimal TipValue { get; set; }

        public decimal TotalPaid { get; set; }

        public IList<MenuItemView> OutstandingMenuItems { get; set; } = new List<MenuItemView>();

        public IList<MenuItemView> ServedMenuItems { get; set; } = new List<MenuItemView>();

        public IList<MenuItemView> RejectedMenuItems { get; set; } = new List<MenuItemView>();

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
            TipValue = @event.TipValue;
        }

        public void Apply(MenuItemsOrdered @event)
        {
            foreach (var item in @event.MenuItems)
            {
                OutstandingMenuItems.Add(new MenuItemView(item));
            }
        }

        public void Apply(MenuItemsServed @event)
        {
            ServedMenuItems.AddRange(@event.MenuItems.Select(i => new MenuItemView(i)));
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
            RejectedItemsValue += @event.MenuItems.Sum(i => i.Price);
            RejectedMenuItems.AddRange(@event.MenuItems.Select(i => new MenuItemView(i)));

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
