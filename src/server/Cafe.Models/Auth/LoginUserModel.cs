using System.ComponentModel.DataAnnotations;

namespace Cafe.Models.Auth
{
    public class LoginUserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
