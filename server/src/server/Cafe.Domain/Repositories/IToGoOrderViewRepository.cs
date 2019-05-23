using Cafe.Domain.Entities;
using Cafe.Domain.Views;
using Optional;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Domain.Repositories
{
    public interface IToGoOrderViewRepository
    {
        Task<Option<ToGoOrderView>> Get(Guid id);

        Task<IList<ToGoOrderView>> GetByStatus(ToGoOrderStatus status);

        Task<IList<ToGoOrderView>> GetAll();
    }
}