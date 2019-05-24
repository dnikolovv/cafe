using Cafe.Core;
using Cafe.Core.MenuContext.Queries;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe.Business.MenuContext.QueryHandlers
{
    public class GetAllMenuItemsHandler : IQueryHandler<GetAllMenuItems, IList<MenuItemView>>
    {
        private readonly IMenuItemViewRepository _menuItemViewRepository;

        public GetAllMenuItemsHandler(IMenuItemViewRepository menuItemViewRepository)
        {
            _menuItemViewRepository = menuItemViewRepository;
        }

        public Task<IList<MenuItemView>> Handle(GetAllMenuItems request, CancellationToken cancellationToken) =>
            _menuItemViewRepository
                .GetAll();
    }
}
