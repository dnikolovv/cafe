using System;
using System.Collections.Generic;

namespace Cafe.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public ICollection<MenuItem> Items { get; set; }
    }
}
