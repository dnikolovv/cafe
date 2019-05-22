using Cafe.Domain.Entities;
using MediatR;
using Optional;
using System.Threading.Tasks;

namespace Cafe.Domain.Repositories
{
    public interface ITableRepository
    {
        Task<Option<Table>> GetByNumber(int tableNumber);

        Task<Unit> Add(Table table);
    }
}
