using AutoMapper;
using Cafe.Core.AuthContext.Commands;
using Cafe.Domain.Entities;

namespace Cafe.Core.AuthContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Register, User>(MemberList.Source)
                .ForMember(d => d.UserName, opts => opts.MapFrom(s => s.Email))
                .ForSourceMember(s => s.Password, opts => opts.DoNotValidate());
        }
    }
}
