using System;

namespace Cafe.Domain.Views
{
    public class TableView
    {
        public int Number { get; set; }

        public Guid WaiterId { get; set; }

        public string WaiterShortName { get; set; }
    }
}
