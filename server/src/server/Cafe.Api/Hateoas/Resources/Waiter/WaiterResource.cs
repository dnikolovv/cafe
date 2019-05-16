using System;
using System.Collections.Generic;

namespace Cafe.Api.Hateoas.Resources.Waiter
{
    public class WaiterResource : Resource
    {
        public Guid Id { get; set; }
        public string ShortName { get; set; }
        public IList<int> TablesServed { get; set; }
    }
}
