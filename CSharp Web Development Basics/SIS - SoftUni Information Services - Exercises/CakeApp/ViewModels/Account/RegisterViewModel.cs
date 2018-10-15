using System.ComponentModel.DataAnnotations;

namespace CakeApp.ViewModels.Account
{
    public class RegisterViewModel
    {
        [StringLength(10, MinimumLength = 6)]
        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}