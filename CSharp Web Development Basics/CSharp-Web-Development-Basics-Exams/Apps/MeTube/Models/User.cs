using System;
using System.Collections.Generic;
using System.Text;

namespace MeTube.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public ICollection<Tube> Tubes { get; set; }
    }
}
