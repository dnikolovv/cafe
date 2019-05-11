using Cafe.Api.Resources;
using RiskFirst.Hateoas;
using System;

namespace Cafe.Api.Hateoas
{
    public interface IPolicy<TResource>
        where TResource : Resource
    {
        Action<LinksPolicyBuilder<TResource>> PolicyConfiguration { get; }
    }
}
