using Cafe.Domain.Entities;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Marten;
using Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cafe.Persistance.Repositories
{
    public class TabViewRepository : ITabViewRepository
    {
        private readonly IDocumentSession _session;

        public TabViewRepository(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<Option<TabView>> Get(Guid id)
        {
            var tab = await _session
                .Query<TabView>()
                .SingleOrDefaultAsync(t => t.Id == id);

            return tab
                .SomeNotNull();
        }

        public async Task<IList<TabView>> GetTabs(Expression<Func<TabView, bool>> predicate = null) =>
            (IList<TabView>)await _session
                .Query<TabView>()
                .Where(predicate ?? (_ => true))
                .ToListAsync();
    }
}
