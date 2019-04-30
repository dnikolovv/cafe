using Cafe.Domain.Views;

namespace Cafe.Domain.Events
{
    public class WaiterHired : IEvent
    {
        public WaiterView Waiter { get; set; }
    }
}
