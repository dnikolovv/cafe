using System;

namespace Cafe.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public MenuItem[] Items { get; set; }
    }
}
