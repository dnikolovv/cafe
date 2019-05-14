using Cafe.Domain.Views;
using System;
using System.Collections.Generic;

namespace Cafe.Api.Hateoas.Resources.Barista
{
    public class BaristaResource : Resource
    {
        public Guid Id { get; set; }

        public string ShortName { get; set; }

        public IList<ToGoOrderView> CompletedOrders { get; set; }
    }
}
