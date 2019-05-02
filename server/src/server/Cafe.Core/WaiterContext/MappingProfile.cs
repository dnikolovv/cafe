using AutoMapper;
using Cafe.Core.WaiterContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;
using System.Linq;

namespace Cafe.Core.WaiterContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HireWaiter, Waiter>(MemberList.Source);
            CreateMap<HireWaiter, WaiterView>(MemberList.Source);
            CreateMap<Waiter, WaiterView>(MemberList.Destination)
                .ForMember(d => d.TablesServed, opts => opts.MapFrom(s => s.ServedTables.Select(t => t.Number)));
        }
    }
}
