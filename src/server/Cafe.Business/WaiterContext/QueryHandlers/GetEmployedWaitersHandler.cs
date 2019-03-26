using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cafe.Core;
using Cafe.Core.WaiterContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Optional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.WaiterContext.QueryHandlers
{
    public class GetEmployedWaitersHandler : IQueryHandler<GetEmployedWaiters, IList<WaiterView>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetEmployedWaitersHandler(IMapper mapper, ApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Option<IList<WaiterView>, Error>> Handle(GetEmployedWaiters request, CancellationToken cancellationToken)
        {
            var waiters = await _dbContext
                .Waiters
                .ProjectTo<WaiterView>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return waiters
                .Some<IList<WaiterView>, Error>();
        }
    }
}
