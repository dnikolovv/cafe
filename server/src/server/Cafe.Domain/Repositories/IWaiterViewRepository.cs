using Cafe.Domain.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Domain.Repositories
{
    public interface IWaiterViewRepository
    {
        Task<IList<WaiterView>> GetAll();
    }
}
