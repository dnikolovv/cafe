using Cafe.Core.CQRS;
using Cafe.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace Cafe.Core.Auth.Commands
{
    public class Register : ICommand<UserModel>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
