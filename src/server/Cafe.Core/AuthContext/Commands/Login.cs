using Cafe.Domain.Views;

namespace Cafe.Core.AuthContext.Commands
{
    public class Login : ICommand<JwtView>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
