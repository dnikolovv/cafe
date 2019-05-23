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
    public class TableViewRepository : ITableViewRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public TableViewRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IList<TableView>> GetAll() =>
            await _dbContext
                .Tables
                .ProjectTo<TableView>(_mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
