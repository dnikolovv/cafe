using Cafe.Domain.Entities;
using Cafe.Domain.Views;
using System.Collections.Generic;

namespace Cafe.Core.OrderContext.Queries
{
    public class GetOrdersByStatus : IQuery<IList<ToGoOrderView>>
    {
        public ToGoOrderStatus Status { get; set; }
    }
}
