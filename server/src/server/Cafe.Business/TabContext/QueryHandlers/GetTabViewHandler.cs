using Cafe.Core;
using Cafe.Core.TabContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Views;
using Marten;
using Optional;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.TabContext.QueryHandlers
{
    public class GetTabViewHandler : IQueryHandler<GetTabView, TabView>
    {
        private readonly IDocumentSession _session;

        public GetTabViewHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<Option<TabView, Error>> Handle(GetTabView request, CancellationToken cancellationToken) =>
            (await _session
                .Query<TabView>()
                .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken))
            .SomeNotNull(Error.NotFound($"No tab with an id of {request.Id} was found."));
    }
}
