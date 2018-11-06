using System.Collections.Generic;

namespace Torshia.Models
{
    public class User
    {
        public User()
        {
            this.Reports = new List<Report>();
        }

        public int  Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public List<Report> Reports { get; set; }
    }
}