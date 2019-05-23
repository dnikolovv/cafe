using Cafe.Domain.Entities;
using MediatR;
using Optional;
using System;
using System.Threading.Tasks;

namespace Cafe.Domain.Repositories
{
    public interface IManagerRepository
    {
        Task<Option<Manager>> Get(Guid id);

        Task<Unit> Add(Manager manager);
    }
}
