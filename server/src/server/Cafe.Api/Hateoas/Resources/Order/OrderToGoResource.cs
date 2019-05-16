using System;

namespace Cafe.Api.Hateoas.Resources.Order
{
    public class OrderToGoResource : Resource
    {
        public Guid OrderId { get; set; }
    }
}
