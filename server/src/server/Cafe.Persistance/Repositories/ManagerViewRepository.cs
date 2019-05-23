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
    public class ManagerViewRepository : IManagerViewRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ManagerViewRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IList<ManagerView>> GetAll() =>
            await _dbContext
                .Managers
                .ProjectTo<ManagerView>(_mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
