using Newtonsoft.Json;

namespace Cafe.Api.Hateoas.Resources.Home
{
    public class ApiRootResource : Resource
    {
        [JsonIgnore]
        public bool IsUserLoggedIn { get; set; }
    }
}
