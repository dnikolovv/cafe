using Cafe.Domain.Views;
using System;

namespace Cafe.Core.OrderContext.Queries
{
    public class GetToGoOrder : IQuery<ToGoOrderView>
    {
        public Guid Id { get; set; }
    }
}
