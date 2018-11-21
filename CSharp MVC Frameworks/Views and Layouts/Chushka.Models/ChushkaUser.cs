using Microsoft.AspNetCore.Identity;

namespace Chushka.Models
{
    public class ChushkaUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}