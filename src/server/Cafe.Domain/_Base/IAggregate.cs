using System;

namespace Cafe.Domain
{
    public interface IAggregate
    {
        Guid Id { get; set; }
    }
}
