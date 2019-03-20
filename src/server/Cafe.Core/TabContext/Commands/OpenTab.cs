using System;

namespace Cafe.Core.TabContext.Commands
{
    public class OpenTab : ICommand
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; }

        public int TableNumber { get; set; }
    }
}
