using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cafe.Core;
using Cafe.Core.BaristaContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Optional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.BaristaContext.QueryHandlers
{
    public class GetEmployedBaristasHandler : IQueryHandler<GetEmployedBaristas, IList<BaristaView>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetEmployedBaristasHandler(IMapper mapper, ApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Option<IList<BaristaView>, Error>> Handle(GetEmployedBaristas request, CancellationToken cancellationToken)
        {
            var baristas = await _dbContext
                .Baristas
                .Include(b => b.CompletedOrders)
                .ProjectTo<BaristaView>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return baristas
                .Some<IList<BaristaView>, Error>();
        }
    }
}
