using System;

namespace Cafe.Api.Hateoas.Resources.Table
{
    public class TableResource : Resource
    {
        public int Number { get; set; }

        public Guid WaiterId { get; set; }

        public string WaiterShortName { get; set; }
    }
}
