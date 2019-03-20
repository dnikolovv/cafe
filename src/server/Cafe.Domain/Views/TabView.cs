using Cafe.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cafe.Domain.Views
{
    public class TabView
    {
        public Guid Id { get; set; }

        public int TableNumber { get; set; }

        public string CustomerName { get; set; }

        public string WaiterName { get; set; }

        public decimal ServedItemsValue { get; set; }

        public bool IsOpen { get; set; }

        public decimal TipValue { get; set; }

        public decimal TotalPaid { get; set; }

        public void ApplyEvent(TabOpened @event)
        {
            IsOpen = true;
            CustomerName = @event.CustomerName;
            WaiterName = @event.WaiterName;
            TableNumber = @event.TableNumber;
        }
    }
}
