using Cafe.Domain.Views;
using Optional;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cafe.Domain.Repositories
{
    public interface ITabViewRepository
    {
        Task<Option<TabView>> Get(Guid id);

        Task<IList<TabView>> GetTabs(Expression<Func<TabView, bool>> predicate = null);
    }
}
