using Cafe.Domain;
using Cafe.Domain.Views;
using Optional;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Core
{
    public interface IUsersService
    {
        Task<IList<UserView>> GetAllUsers();

        Task<Option<UserView, Error>> GetUser(Guid id);
    }
}
