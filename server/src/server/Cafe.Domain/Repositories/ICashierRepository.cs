using Cafe.Domain.Entities;
using MediatR;
using Optional;
using System;
using System.Threading.Tasks;

namespace Cafe.Domain.Repositories
{
    public interface ICashierRepository
    {
        Task<Option<Cashier>> Get(Guid id);

        Task<Unit> Add(Cashier cashier);
    }
}
