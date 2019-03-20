using Cafe.Models.Auth;

namespace Cafe.Core.AuthContext.Commands
{
    public class Login : ICommand<JwtModel>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
