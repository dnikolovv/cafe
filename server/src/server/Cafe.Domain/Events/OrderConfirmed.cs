using Cafe.Domain.Views;

namespace Cafe.Domain.Events
{
    public class OrderConfirmed : IEvent
    {
        public ToGoOrderView Order { get; set; }
    }
}
