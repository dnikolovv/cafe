using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cafe.Core;
using Cafe.Core.CashierContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Optional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.CashierContext.QueryHandlers
{
    public class GetEmployedCashiersHandler : IQueryHandler<GetEmployedCashiers, IList<CashierView>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetEmployedCashiersHandler(IMapper mapper, ApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Option<IList<CashierView>, Error>> Handle(GetEmployedCashiers request, CancellationToken cancellationToken)
        {
            var cashiers = await _dbContext
                .Cashiers
                .ProjectTo<CashierView>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return cashiers
                .Some<IList<CashierView>, Error>();
        }
    }
}
