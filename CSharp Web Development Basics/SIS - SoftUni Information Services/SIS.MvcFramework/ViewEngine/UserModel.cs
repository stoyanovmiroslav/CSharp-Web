using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.ViewEngine
{
    public class UserModel
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }

        public string Email { get; set; }

        public bool Exist => this.Name != null;
    }
}