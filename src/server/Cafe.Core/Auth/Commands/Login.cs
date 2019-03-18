using Cafe.Core.CQRS;
using Cafe.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace Cafe.Core.Auth.Commands
{
    public class Login : ICommand<JwtModel>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
