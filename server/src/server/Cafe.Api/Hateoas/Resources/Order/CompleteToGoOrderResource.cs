using System;

namespace Cafe.Api.Hateoas.Resources.Order
{
    public class CompleteToGoOrderResource : Resource
    {
        public Guid OrderId { get; set; }
    }
}
