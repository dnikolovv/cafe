using AutoMapper;
using Cafe.Domain.Views;

namespace Cafe.Api.Resources.Auth
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<JwtView, LoginResource>(MemberList.Destination);
            CreateMap<UserView, UserResource>(MemberList.Destination);
        }
    }
}
