using System;

namespace Cafe.Core.OrderContext.Commands
{
    public class CompleteToGoOrder : ICommand
    {
        public Guid OrderId { get; set; }
    }
}
