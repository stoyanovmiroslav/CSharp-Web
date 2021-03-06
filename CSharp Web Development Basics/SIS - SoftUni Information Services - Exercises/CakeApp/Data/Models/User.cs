﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CakeApp.Data.Models
{
    public class User
    {
        public User()
        {
            this.Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime DateOfRegistration { get; set; } = DateTime.UtcNow;

        public ICollection<Order> Orders  { get; set; }
    }
}
