using Cafe.Domain.Events;
using Marten.Events.Projections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cafe.Domain.Views
{
    public class TabViewProjection : ViewProjection<TabView, Guid>
    {
        public TabViewProjection()
        {
            ProjectEvent<TabOpened>((ev) => ev.TabId, (view, @event) => view.ApplyEvent(@event));
        }
    }
}
