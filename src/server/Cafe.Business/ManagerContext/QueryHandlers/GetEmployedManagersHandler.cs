using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cafe.Core;
using Cafe.Core.ManagerContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Views;
using Cafe.Persistance.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Optional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.ManagerContext.QueryHandlers
{
    public class GetEmployedManagersHandler : IQueryHandler<GetEmployedManagers, IList<ManagerView>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetEmployedManagersHandler(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Option<IList<ManagerView>, Error>> Handle(GetEmployedManagers request, CancellationToken cancellationToken)
        {
            var managers = await _dbContext
                .Managers
                .ProjectTo<ManagerView>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return managers
                .Some<IList<ManagerView>, Error>();
        }
    }
}
