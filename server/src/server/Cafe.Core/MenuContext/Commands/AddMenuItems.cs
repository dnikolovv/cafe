using Cafe.Domain.Views;
using System.Collections.Generic;

namespace Cafe.Core.MenuContext.Commands
{
    public class AddMenuItems : ICommand
    {
        public IList<MenuItemView> MenuItems { get; set; }
    }
}
