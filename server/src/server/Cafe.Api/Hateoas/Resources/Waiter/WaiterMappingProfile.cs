using AutoMapper;
using Cafe.Domain.Views;

namespace Cafe.Api.Hateoas.Resources.Waiter
{
    public class WaiterMappingProfile : Profile
    {
        public WaiterMappingProfile()
        {
            CreateMap<WaiterView, WaiterResource>(MemberList.Destination);
        }
    }
}
