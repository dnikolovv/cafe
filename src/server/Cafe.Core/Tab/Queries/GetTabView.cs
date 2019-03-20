using Cafe.Domain.Views;
using System;

namespace Cafe.Core.Tab.Queries
{
    public class GetTabView : IQuery<TabView>
    {
        public Guid Id { get; set; }
    }
}
