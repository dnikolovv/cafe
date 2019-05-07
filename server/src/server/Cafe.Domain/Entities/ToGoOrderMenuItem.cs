using System;

namespace Cafe.Domain.Entities
{
    public class ToGoOrderMenuItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public ToGoOrder Order { get; set; }
        public Guid MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
