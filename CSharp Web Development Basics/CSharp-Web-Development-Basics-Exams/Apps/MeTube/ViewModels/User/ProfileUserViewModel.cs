using System.Collections.Generic;

namespace MeTube.ViewModels.User
{
    public class ProfileUserViewModel
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public List<MeTube.Models.Tube> Tubes { get; set; }
    }
}
