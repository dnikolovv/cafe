using Cafe.Domain.Entities;
using Cafe.Domain.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafe.Api.Hateoas.Resources.Order
{
    public class ToGoOrderResource : Resource
    {
        public Guid Id { get; set; }

        public IList<MenuItemView> OrderedItems { get; set; }

        public ToGoOrderStatus Status { get; set; }

        public DateTime Date { get; set; }

        public string StatusText => Enum.GetName(typeof(ToGoOrderStatus), Status);

        public decimal Price => OrderedItems?.Sum(i => i.Price) ?? 0;
    }
}
