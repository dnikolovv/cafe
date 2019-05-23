using Cafe.Core;
using Cafe.Core.MenuContext.Queries;
using Cafe.Domain;
using Cafe.Domain.Repositories;
using Cafe.Domain.Views;
using Optional;
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

        public async Task<Option<IList<MenuItemView>, Error>> Handle(GetAllMenuItems request, CancellationToken cancellationToken)
        {
            var items = await _menuItemViewRepository
                .GetAll();

            return items
                .Some<IList<MenuItemView>, Error>();
        }
    }
}
