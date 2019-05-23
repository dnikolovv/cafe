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
    public class CashierViewRepository : ICashierViewRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CashierViewRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IList<CashierView>> GetAll() =>
            await _dbContext
                .Cashiers
                .ProjectTo<CashierView>(_mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
