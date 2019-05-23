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
    public class MenuItemViewRepository : IMenuItemViewRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public MenuItemViewRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IList<MenuItemView>> GetAll() =>
            await _dbContext
                .MenuItems
                .ProjectTo<MenuItemView>(_mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
