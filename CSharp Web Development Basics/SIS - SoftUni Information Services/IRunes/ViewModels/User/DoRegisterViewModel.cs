namespace IRunes.ViewModels.User
{
    public class DoRegisterViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Email { get; internal set; }
    }
}