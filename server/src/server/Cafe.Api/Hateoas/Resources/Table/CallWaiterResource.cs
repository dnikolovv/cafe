using Newtonsoft.Json;

namespace Cafe.Api.Hateoas.Resources.Table
{
    public class CallWaiterResource : Resource
    {
        [JsonIgnore]
        public int TableNumber { get; set; }
    }
}
