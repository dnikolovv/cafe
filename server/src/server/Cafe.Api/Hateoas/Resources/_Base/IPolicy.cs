using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas.Resources
{
    public interface IPolicy<TResource>
        where TResource : Resource
    {
        Action<LinksPolicyBuilder<TResource>> PolicyConfiguration { get; }
    }
}
