using Cafe.Domain.Views;
using System;

namespace Cafe.Core.WaiterContext.Commands
{
    public class HireWaiter : ICommand<WaiterView>
    {
        public Guid Id { get; set; }

        public string ShortName { get; set; }
    }
}
