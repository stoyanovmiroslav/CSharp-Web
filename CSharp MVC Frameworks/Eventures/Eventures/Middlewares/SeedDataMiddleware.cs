using Eventures.Data;
using Eventures.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Eventures.Middlewares
{
    public class SeedDataMiddleware
    {
        private const string ADMIN_ROLE = "Admin";

        private readonly RequestDelegate _next;

        public SeedDataMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<EventuresUser> userManager,
                                      RoleManager<EventuresRole> roleManager, EventuresDbContext db)
        {
            SeedRoles(roleManager).GetAwaiter().GetResult();

            SeedUserInRoles(userManager).GetAwaiter().GetResult();

            await _next(context);
        }

        private static async Task SeedRoles(RoleManager<EventuresRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(ADMIN_ROLE))
            {
                await roleManager.CreateAsync(new EventuresRole(ADMIN_ROLE));
            }
        }

        private static async Task SeedUserInRoles(UserManager<EventuresUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new EventuresUser
                {
                    UserName = "AdminUser",
                    Email = "admin@gmail.com",
                    FirstName = "AdminFirstName",
                    LastName = "AdminLastName",
                    UniqueCitizenNumber = "1234567890"
                };

                var password = "123456";

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, ADMIN_ROLE);
                }
            }
        }
    }
}