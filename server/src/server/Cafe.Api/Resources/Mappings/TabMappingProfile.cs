using AutoMapper;
using Cafe.Domain.Views;

namespace Cafe.Api.Resources.Mappings
{
    public class TabMappingProfile : Profile
    {
        public TabMappingProfile()
        {
            CreateMap<TabView, TabResource>(MemberList.Destination);
        }
    }
}
