using System;
using System.Collections.Generic;

namespace Cafe.Domain.Entities
{
    public class Waiter
    {
        public Guid Id { get; set; }

        public string ShortName { get; set; }

        public IList<Table> ServedTables { get; set; }
    }
}
