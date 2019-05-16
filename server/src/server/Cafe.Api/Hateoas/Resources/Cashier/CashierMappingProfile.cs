using AutoMapper;
using Cafe.Domain.Views;

namespace Cafe.Api.Hateoas.Resources.Cashier
{
    public class CashierMappingProfile : Profile
    {
        public CashierMappingProfile()
        {
            CreateMap<CashierView, CashierResource>(MemberList.Destination);
        }
    }
}
