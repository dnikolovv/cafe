using Cafe.Core;
using Cafe.Core.TabContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Optional;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.TabContext.QueryHandlers
{
    public class GetTabViewHandler : IQueryHandler<GetTabView, Option<TabView, Error>>
    {
        private readonly ITabViewRepository _tabViewRepository;

        public GetTabViewHandler(ITabViewRepository tabViewRepository)
        {
            _tabViewRepository = tabViewRepository;
        }

        public async Task<Option<TabView, Error>> Handle(GetTabView request, CancellationToken cancellationToken) =>
            (await _tabViewRepository
                .Get(request.Id))
                .WithException(Error.NotFound($"No tab with an id of {request.Id} was found."));
    }
}
