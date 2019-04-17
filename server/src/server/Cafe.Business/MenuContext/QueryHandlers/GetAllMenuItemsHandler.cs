using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cafe.Core;
using Cafe.Core.MenuContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Optional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.MenuContext.QueryHandlers
{
    public class GetAllMenuItemsHandler : IQueryHandler<GetAllMenuItems, IList<MenuItemView>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllMenuItemsHandler(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Option<IList<MenuItemView>, Error>> Handle(GetAllMenuItems request, CancellationToken cancellationToken)
        {
            var items = await _dbContext
                .MenuItems
                .ProjectTo<MenuItemView>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return items
                .Some<IList<MenuItemView>, Error>();
        }
    }
}
