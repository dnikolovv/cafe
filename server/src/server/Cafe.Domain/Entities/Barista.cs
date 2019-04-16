using System;
using System.Collections.Generic;

namespace Cafe.Domain.Entities
{
    public class Barista
    {
        public Guid Id { get; set; }

        public string ShortName { get; set; }

        public ICollection<ToGoOrder> CompletedOrders { get; set; }
    }
}
