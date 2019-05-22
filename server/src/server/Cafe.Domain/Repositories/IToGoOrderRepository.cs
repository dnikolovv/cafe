using Cafe.Domain.Entities;
using MediatR;
using Optional;
using System;
using System.Threading.Tasks;

namespace Cafe.Domain.Repositories
{
    public interface IToGoOrderRepository
    {
        Task<Option<ToGoOrder>> Get(Guid id);

        Task<Unit> Add(ToGoOrder order);

        Task<Unit> Update(ToGoOrder order);
    }
}
