using System;
using System.Collections.Generic;

namespace Cafe.Domain.Entities
{
    public class ToGoOrder
    {
        public Guid Id { get; set; }

        public ICollection<MenuItem> OrderedItems { get; set; }

        public ToGoOrderStatus Status { get; set; }
    }
}
