using Cafe.Domain.Entities;
using Optional;
using System;
using System.Threading.Tasks;

namespace Cafe.Domain.Repositories
{
    public interface ITabRepository
    {
        Task<Option<Tab>> Get(Guid id);
    }
}
