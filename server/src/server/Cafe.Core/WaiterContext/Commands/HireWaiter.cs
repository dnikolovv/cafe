using System;

namespace Cafe.Core.WaiterContext.Commands
{
    public class HireWaiter : ICommand
    {
        public Guid Id { get; set; }

        public string ShortName { get; set; }
    }
}
