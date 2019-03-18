using AutoMapper;
using Cafe.Domain.Entities;
using Cafe.Models.Auth;

namespace Cafe.Models.Mappings
{
    public class UsersMapping : Profile
    {
        public UsersMapping()
        {
            CreateMap<User, UserModel>(MemberList.Destination);
        }
    }
}
