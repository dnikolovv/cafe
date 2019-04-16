using Newtonsoft.Json;
using Optional;
using System;

namespace Cafe.Core.OrderContext.Commands
{
    public class CompleteToGoOrder : ICommand
    {
        public Guid OrderId { get; set; }

        [JsonIgnore]
        public Option<Guid> BaristaId { get; set; }
    }
}
