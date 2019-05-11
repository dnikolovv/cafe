using AutoMapper;
using Cafe.Domain.Views;
using System.Collections.Generic;

namespace Cafe.Api.Resources.Mappings
{
    public class TabMappingProfile : Profile
    {
        public TabMappingProfile()
        {
            CreateMap<TabView, TabResource>(MemberList.Destination);

            CreateMap<IList<TabResource>, TabsResource>()
                .ForMember(d => d.Tabs, opts => opts.MapFrom(s => s));
        }
    }
}
