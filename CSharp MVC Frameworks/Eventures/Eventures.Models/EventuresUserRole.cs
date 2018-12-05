using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventures.Models
{
    public class EventuresUserRole : IdentityUserRole<string>
    {
        public virtual EventuresUser User { get; set; }

        public virtual EventuresRole Role { get; set; }
    }
}