using AutoMapper;
using Cafe.Domain.Views;

namespace Cafe.Api.Hateoas.Resources.Menu
{
    public class MenuMappingProfile : Profile
    {
        public MenuMappingProfile()
        {
            CreateMap<MenuItemView, MenuItemResource>(MemberList.Destination);
        }
    }
}
