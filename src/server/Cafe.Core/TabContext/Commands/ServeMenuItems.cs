using System;
using System.Collections.Generic;

namespace Cafe.Core.TabContext.Commands
{
    public class ServeMenuItems : ICommand
    {
        public Guid TabId { get; set; }

        public IList<int> ItemNumbers { get; set; }
    }
}
