using System;
using System.Collections.Generic;
using System.Text;

namespace Eventures.Models.ViewModel
{
    public class AdminUserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public ICollection<AdminUserRoleViewModel> UserRoles { get; set; }
    }
}
