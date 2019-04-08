using Newtonsoft.Json;
using System;

namespace Cafe.Core.OrderContext.Commands
{
    public class CompleteToGoOrder : ICommand
    {
        public Guid OrderId { get; set; }

        [JsonIgnore]
        public Guid? BaristaId { get; set; }
    }
}
