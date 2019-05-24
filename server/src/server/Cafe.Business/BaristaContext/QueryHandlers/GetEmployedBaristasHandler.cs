using Cafe.Core;
using Cafe.Core.BaristaContext.Queries;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.BaristaContext.QueryHandlers
{
    public class GetEmployedBaristasHandler : IQueryHandler<GetEmployedBaristas, IList<BaristaView>>
    {
        private readonly IBaristaViewRepository _baristaViewRepository;

        public GetEmployedBaristasHandler(IBaristaViewRepository baristaViewRepository)
        {
            _baristaViewRepository = baristaViewRepository;
        }

        public Task<IList<BaristaView>> Handle(GetEmployedBaristas request, CancellationToken cancellationToken) =>
            _baristaViewRepository
                .GetAll();
    }
}
