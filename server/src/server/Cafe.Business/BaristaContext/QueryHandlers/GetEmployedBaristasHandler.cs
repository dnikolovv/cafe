using Cafe.Core;
using Cafe.Core.BaristaContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Optional;
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

        public async Task<Option<IList<BaristaView>, Error>> Handle(GetEmployedBaristas request, CancellationToken cancellationToken)
        {
            var baristas = await _baristaViewRepository
                .GetAll();

            return baristas
                .Some<IList<BaristaView>, Error>();
        }
    }
}
