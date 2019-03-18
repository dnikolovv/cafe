using Cafe.Domain;
using Cafe.Models.Auth;
using Optional;
using System.Threading.Tasks;

namespace Cafe.Core.Auth.Services
{
    public interface IAuthService
    {
        Task<Option<JwtModel, Error>> Login(LoginUserModel model);

        Task<Option<UserModel, Error>> Register(RegisterUserModel model);
    }
}
