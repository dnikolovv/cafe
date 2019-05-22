using Cafe.Domain.Entities;
using MediatR;
using Optional;
using System;
using System.Threading.Tasks;

namespace Cafe.Domain.Repositories
{
    public interface IBaristaRepository
    {
        Task<Option<Barista>> Get(Guid id);

        Task<Unit> Update(Barista barista);
    }
}
