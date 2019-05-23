using Cafe.Domain.Entities;
using Cafe.Domain.Repositories;
using Cafe.Persistance.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Optional;
using System;
using System.Threading.Tasks;

namespace Cafe.Persistance.Repositories
{
    public class WaiterRepository : IWaiterRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public WaiterRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Add(Waiter waiter)
        {
            await _dbContext.AddAsync(waiter);
            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }

        public async Task<Option<Waiter>> Get(Guid id) =>
            (await _dbContext
                .Waiters
                .Include(w => w.ServedTables)
                .FirstOrDefaultAsync(w => w.Id == id))
            .SomeNotNull();
    }
}
