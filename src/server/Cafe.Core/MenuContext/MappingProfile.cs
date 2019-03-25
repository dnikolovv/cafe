using AutoMapper;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;

namespace Cafe.Core.MenuContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MenuItemView, MenuItem>(MemberList.Source);
        }
    }
}
