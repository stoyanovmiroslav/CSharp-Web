﻿using FluffyCats.Models;

namespace FluffyCats.ViewModels.Account
{
    public class UserViewModel
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public Role Role { get; set; }
    }
}