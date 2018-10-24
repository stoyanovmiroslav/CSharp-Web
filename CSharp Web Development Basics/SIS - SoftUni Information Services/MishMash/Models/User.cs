using MishMash.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MishMash.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public ICollection<UserChanel> Channels { get; set; }

        public Role Role { get; set; }
    }
}