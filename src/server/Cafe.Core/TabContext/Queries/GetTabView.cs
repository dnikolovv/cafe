using Cafe.Domain.Views;
using System;

namespace Cafe.Core.TabContext.Queries
{
    public class GetTabView : IQuery<TabView>
    {
        public Guid Id { get; set; }
    }
}
