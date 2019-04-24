using Cafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafe.Domain.Views
{
    public class ToGoOrderView
    {
        public Guid Id { get; set; }

        public IList<MenuItemView> OrderedItems { get; set; }

        public ToGoOrderStatus Status { get; set; }

        public DateTime Date { get; set; }

        public string StatusText => Enum.GetName(typeof(ToGoOrderStatus), Status);

        public decimal Price => OrderedItems?.Sum(i => i.Price) ?? 0;
    }
}
