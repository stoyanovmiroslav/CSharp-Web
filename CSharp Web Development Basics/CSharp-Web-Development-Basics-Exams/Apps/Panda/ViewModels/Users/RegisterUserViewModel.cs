using Panda.Models;

namespace Users.ViewModels.Users
{
    public class RegisterUserViewModel
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public Role Role { get; set; }
    }
}