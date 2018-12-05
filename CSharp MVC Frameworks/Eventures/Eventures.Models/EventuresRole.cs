using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Eventures.Models
{
    public class EventuresRole : IdentityRole
    {
        public EventuresRole()
        {
        }

        public EventuresRole(string roleName) 
            : base(roleName)
        {
        }

        public ICollection<EventuresUserRole> UserRoles { get; set; }
    }
}