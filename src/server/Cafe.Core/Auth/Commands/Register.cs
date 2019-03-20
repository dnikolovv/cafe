using Cafe.Models.Auth;

namespace Cafe.Core.Auth.Commands
{
    public class Register : ICommand<UserModel>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
