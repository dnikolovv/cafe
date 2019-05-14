using System;

namespace Cafe.Api.Hateoas.Resources.Order
{
    public class ConfirmToGoOrderResource : Resource
    {
        public Guid OrderId { get; set; }
    }
}
