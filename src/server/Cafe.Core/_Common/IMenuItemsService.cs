using Cafe.Domain;
using Cafe.Domain.Entities;
using Optional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Core
{
    public interface IMenuItemsService
    {
        Task<Option<IList<MenuItem>, Error>> ItemsShouldExist(IList<int> menuItemNumbers);
    }
}
