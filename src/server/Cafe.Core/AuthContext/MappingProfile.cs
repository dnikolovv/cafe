using AutoMapper;
using Cafe.Core.AuthContext.Commands;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;

namespace Cafe.Core.AuthContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Register, User>(MemberList.Source)
                .ForMember(d => d.UserName, opts => opts.MapFrom(s => s.Email))
                .ForSourceMember(s => s.Password, opts => opts.DoNotValidate());

            CreateMap<User, UserView>(MemberList.Destination)
                .ForMember(d => d.WaiterId, opts => opts.Ignore())
                .ForMember(d => d.ManagerId, opts => opts.Ignore());
        }
    }
}
