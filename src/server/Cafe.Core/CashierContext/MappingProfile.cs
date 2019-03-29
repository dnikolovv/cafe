using AutoMapper;
using Cafe.Core.CashierContext.Commands;
using Cafe.Domain.Entities;

namespace Cafe.Core.CashierContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HireCashier, Cashier>(MemberList.Source);
        }
    }
}
