using AutoMapper;
using Cafe.Domain.Views;

namespace Cafe.Api.Hateoas.Resources.Barista
{
    public class BaristaMappingProfile : Profile
    {
        public BaristaMappingProfile()
        {
            CreateMap<BaristaView, BaristaResource>(MemberList.Destination);
        }
    }
}
