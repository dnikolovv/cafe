using AutoMapper;
using Cafe.Domain.Views;

namespace Cafe.Api.Hateoas.Resources.Table
{
    public class TableMappingProfile : Profile
    {
        public TableMappingProfile()
        {
            CreateMap<TableView, TableResource>(MemberList.Destination);
        }
    }
}
