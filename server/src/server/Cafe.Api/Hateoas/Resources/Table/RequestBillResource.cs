using Newtonsoft.Json;

namespace Cafe.Api.Hateoas.Resources.Table
{
    public class RequestBillResource : Resource
    {
        [JsonIgnore]
        public int TableNumber { get; set; }
    }
}
