using MishMash.Models.Enums;

namespace MishMash.ViewModels.Account
{ 
    public class RegisterViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }
    }
}