using System;
using System.Collections.Generic;

namespace Cafe.Domain.Views
{
    public class WaiterView
    {
        public Guid Id { get; set; }
        public string ShortName { get; set; }
        public IList<int> TablesServed { get; set; }
    }
}
