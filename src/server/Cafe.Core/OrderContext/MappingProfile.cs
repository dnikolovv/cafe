using AutoMapper;
using Cafe.Core.OrderContext.Commands;
using Cafe.Domain.Entities;

namespace Cafe.Core.OrderContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderToGo, Order>(MemberList.Source);
        }
    }
}
