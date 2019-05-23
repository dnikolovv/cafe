using Cafe.Domain.Views;
using Optional;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Domain.Repositories
{
    public interface IUserViewRepository
    {
        Task<IList<UserView>> GetAll();

        Task<Option<UserView>> Get(Guid id);
    }
}
