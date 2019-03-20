using System;

namespace Cafe.Core.Tab.Commands
{
    public class OpenTab : ICommand
    {
        public Guid Id { get; set; }

        public string ClientName { get; set; }

        public int TableNumber { get; set; }
    }
}
