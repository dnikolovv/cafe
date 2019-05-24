using Cafe.Core;
using Cafe.Core.TabContext.Queries;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
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

        public Task<IList<TabView>> Handle(GetTabHistory request, CancellationToken cancellationToken) =>
            _tabViewRepository
                .GetTabs(t => !t.IsOpen);
    }
}
