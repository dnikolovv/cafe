using AutoMapper;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;

namespace Cafe.Core.TableContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Table, TableView>(MemberList.Destination);
        }
    }
}
