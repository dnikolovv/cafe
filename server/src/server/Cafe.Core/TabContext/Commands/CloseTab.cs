using System;

namespace Cafe.Core.TabContext.Commands
{
    public class CloseTab : ICommand
    {
        public Guid TabId { get; set; }

        public decimal AmountPaid { get; set; }
    }
}
