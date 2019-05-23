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
    public class ManagerRepository : IManagerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ManagerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Add(Manager manager)
        {
            _dbContext.Managers.Add(manager);
            await _dbContext.SaveChangesAsync();
            return Unit.Value;
        }

        public async Task<Option<Manager>> Get(Guid id) =>
            (await _dbContext
                .Managers
                .FirstOrDefaultAsync(m => m.Id == id))
            .SomeNotNull();
    }
}
