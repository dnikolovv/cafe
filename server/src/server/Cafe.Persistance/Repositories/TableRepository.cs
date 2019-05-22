using Cafe.Domain.Entities;
using Cafe.Domain.Repositories;
using Cafe.Persistance.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;
using System.Threading.Tasks;

namespace Cafe.Persistance.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TableRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Add(Table table)
        {
            _dbContext.Add(table);
            await _dbContext.SaveChangesAsync();
            return Unit.Value;
        }

        public Task<Option<Table>> GetByNumber(int tableNumber) =>
            _dbContext
                .Tables
                .Include(t => t.Waiter)
                .FirstOrDefaultAsync(t => t.Number == tableNumber)
                .SomeNotNull();
    }
}
