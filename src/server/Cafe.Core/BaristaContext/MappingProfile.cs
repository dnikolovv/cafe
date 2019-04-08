using AutoMapper;
using Cafe.Core.BaristaContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;

namespace Cafe.Core.BaristaContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HireBarista, Barista>(MemberList.Source);

            CreateMap<Barista, BaristaView>(MemberList.Destination);
        }
    }
}
