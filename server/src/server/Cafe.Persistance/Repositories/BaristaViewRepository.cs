using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Persistance.Repositories
{
    public class BaristaViewRepository : IBaristaViewRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public BaristaViewRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IList<BaristaView>> GetAll() =>
            await _dbContext
                .Baristas
                .Include(b => b.CompletedOrders)
                .ProjectTo<BaristaView>(_mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
