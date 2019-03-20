using AutoMapper;
using Cafe.Core.TableContext.Commands;
using Cafe.Domain.Entities;

namespace Cafe.Core.TabContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddTable, Table>(MemberList.Source);
        }
    }
}
