using System.Collections.Generic;

namespace Cafe.Api.Resources
{
    public class TabsResource : Resource
    {
        public IEnumerable<TabResource> Tabs { get; set; }
    }
}
