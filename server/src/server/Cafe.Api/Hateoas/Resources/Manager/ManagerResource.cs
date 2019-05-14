using System;

namespace Cafe.Api.Hateoas.Resources.Manager
{
    public class ManagerResource : Resource
    {
        public Guid Id { get; set; }

        public string ShortName { get; set; }
    }
}
