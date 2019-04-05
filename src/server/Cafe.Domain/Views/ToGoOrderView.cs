using Cafe.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cafe.Domain.Views
{
    public class ToGoOrderView
    {
        public Guid Id { get; set; }

        public IList<MenuItemView> OrderedItems { get; set; }

        public ToGoOrderStatus Status { get; set; }
    }
}
