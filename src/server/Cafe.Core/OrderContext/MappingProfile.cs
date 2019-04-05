using AutoMapper;
using Cafe.Core.OrderContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;

namespace Cafe.Core.OrderContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderToGo, ToGoOrder>(MemberList.Source);

            CreateMap<ToGoOrder, ToGoOrderView>(MemberList.Destination);
        }
    }
}
