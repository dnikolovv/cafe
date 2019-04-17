using AutoMapper;
using Cafe.Core.CashierContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;

namespace Cafe.Core.CashierContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HireCashier, Cashier>(MemberList.Source);
            CreateMap<Cashier, CashierView>(MemberList.Destination);
        }
    }
}
