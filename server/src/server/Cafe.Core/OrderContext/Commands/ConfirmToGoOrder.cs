using System;

namespace Cafe.Core.OrderContext.Commands
{
    public class ConfirmToGoOrder : ICommand
    {
        public Guid OrderId { get; set; }

        public decimal PricePaid { get; set; }
    }
}
