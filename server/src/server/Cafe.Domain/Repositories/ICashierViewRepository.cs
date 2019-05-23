using Cafe.Domain.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Domain.Repositories
{
    public interface ICashierViewRepository
    {
        Task<IList<CashierView>> GetAll();
    }
}
