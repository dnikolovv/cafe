using Cafe.Domain;
using Cafe.Domain.Views;
using Optional;
using System;

namespace Cafe.Core.OrderContext.Queries
{
    public class GetToGoOrder : IQuery<Option<ToGoOrderView, Error>>
    {
        public Guid Id { get; set; }
    }
}
