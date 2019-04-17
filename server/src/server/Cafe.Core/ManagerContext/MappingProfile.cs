using AutoMapper;
using Cafe.Core.ManagerContext.Commands;
using Cafe.Domain.Entities;

namespace Cafe.Core.ManagerContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HireManager, Manager>(MemberList.Source);
        }
    }
}
