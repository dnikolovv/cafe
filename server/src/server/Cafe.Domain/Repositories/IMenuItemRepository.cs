using Cafe.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cafe.Domain.Repositories
{
    public interface IMenuItemRepository
    {
        Task<Unit> Add(params MenuItem[] item);

        Task<IList<MenuItem>> GetAll(Expression<Func<MenuItem, bool>> predicate = null);
    }
}