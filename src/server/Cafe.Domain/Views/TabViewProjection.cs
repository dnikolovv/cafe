using Cafe.Domain.Events;
using Marten.Events.Projections;
using System;

namespace Cafe.Domain.Views
{
    public class TabViewProjection : ViewProjection<TabView, Guid>
    {
        public TabViewProjection()
        {
            ProjectEvent<TabOpened>(ev => ev.TabId, (view, @event) => view.Apply(@event));
            ProjectEvent<MenuItemsOrdered>(ev => ev.TabId, (view, @event) => view.Apply(@event));
        }
    }
}
