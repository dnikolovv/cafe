using Cafe.Core.CQRS;
using Cafe.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace Cafe.Core.Auth.Commands
{
    public class Login : ICommand<JwtModel>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
