using System;
using System.Collections.Generic;

namespace Cafe.Core.TabContext.Commands
{
    public class OrderMenuItems : ICommand
    {
        public Guid TabId { get; set; }

        public IList<int> ItemNumbers { get; set; }
    }
}
