using AutoMapper;
using Cafe.Domain.Views;

namespace Cafe.Api.Hateoas.Resources.Order
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<ToGoOrderView, ToGoOrderResource>(MemberList.Destination);
        }
    }
}
