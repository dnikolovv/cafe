using Cafe.Core;
using Cafe.Core.TabContext.Queries;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.TabContext.QueryHandlers
{
    public class GetAllOpenTabsHandler : IQueryHandler<GetAllOpenTabs, IList<TabView>>
    {
        private readonly ITabViewRepository _tabViewRepository;

        public GetAllOpenTabsHandler(ITabViewRepository tabViewRepository)
        {
            _tabViewRepository = tabViewRepository;
        }

        public Task<IList<TabView>> Handle(GetAllOpenTabs request, CancellationToken cancellationToken) =>
            _tabViewRepository
                .GetTabs(t => t.IsOpen);
    }
}
