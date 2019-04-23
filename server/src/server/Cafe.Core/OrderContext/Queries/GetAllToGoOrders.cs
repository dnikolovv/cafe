using Cafe.Domain.Views;
using System.Collections.Generic;

namespace Cafe.Core.OrderContext.Queries
{
    public class GetAllToGoOrders : IQuery<IList<ToGoOrderView>>
    {
    }
}
