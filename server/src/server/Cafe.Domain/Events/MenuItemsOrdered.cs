using Cafe.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cafe.Domain.Events
{
    public class MenuItemsOrdered : IEvent
    {
        public Guid TabId { get; set; }

        public IList<MenuItem> MenuItems { get; set; }
    }
}
