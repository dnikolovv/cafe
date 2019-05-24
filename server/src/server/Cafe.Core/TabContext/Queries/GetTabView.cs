using Cafe.Domain;
using Cafe.Domain.Views;
using Optional;
using System;

namespace Cafe.Core.TabContext.Queries
{
    public class GetTabView : IQuery<Option<TabView, Error>>
    {
        public Guid Id { get; set; }
    }
}
