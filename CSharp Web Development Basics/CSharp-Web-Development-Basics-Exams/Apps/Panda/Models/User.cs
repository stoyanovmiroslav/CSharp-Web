using System.Collections.Generic;

namespace Panda.Models
{
    public class User
    {
        public int  Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }

        public ICollection<Receipt> Receipts { get; set; } = new HashSet<Receipt>();

        public ICollection<Package> Packages { get; set; } = new HashSet<Package>();
    }
}