using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class ChushkaUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}