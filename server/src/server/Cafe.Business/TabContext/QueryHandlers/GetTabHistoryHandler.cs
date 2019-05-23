using Cafe.Core;
using Cafe.Core.TabContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Optional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.TabContext.QueryHandlers
{
    public class GetTabHistoryHandler : IQueryHandler<GetTabHistory, IList<TabView>>
    {
        private readonly ITabViewRepository _tabViewRepository;

        public GetTabHistoryHandler(ITabViewRepository tabViewRepository)
        {
            _tabViewRepository = tabViewRepository;
        }

        public async Task<Option<IList<TabView>, Error>> Handle(GetTabHistory request, CancellationToken cancellationToken)
        {
            var tabs = await _tabViewRepository
                .GetTabs(t => !t.IsOpen);

            return tabs.Some<IList<TabView>, Error>();
        }
    }
}
