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
            CreateMap<OrderToGo, ToGoOrder>(MemberList.Source)
                .ForMember(d => d.Status, opts => opts.MapFrom(_ => ToGoOrderStatus.Pending))
                .ForSourceMember(s => s.ItemNumbers, opts => opts.DoNotValidate());

            CreateMap<ToGoOrder, ToGoOrderView>(MemberList.Destination);

            CreateMap<MenuItem, MenuItemView>(MemberList.Destination);

            CreateMap<ToGoOrderMenuItem, MenuItemView>(MemberList.Destination)
                .ConvertUsing((s, d, ctx) => ctx.Mapper.Map<MenuItemView>(s.MenuItem));
        }
    }
}
