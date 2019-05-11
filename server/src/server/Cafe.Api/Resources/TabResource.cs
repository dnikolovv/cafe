using Cafe.Domain.Views;
using System;
using System.Collections.Generic;

namespace Cafe.Api.Resources
{
    public class TabResource : Resource
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
    }
}
