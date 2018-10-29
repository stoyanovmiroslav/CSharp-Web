using Microsoft.EntityFrameworkCore;
using SIS.Framework.ActionResult.Contracts;
using SIS.Framework.Attributes;
using System.Linq;

namespace CakeApp.Controllers
{
    public class UserController : BaseController
    {
        [Authorize]
        public IActionResult Profile()
        {
            if (this.User == null)
            {
                return this.BadRequestError("You need to login first!", "Account/Login");
            }

            var user = db.Users.Where(x => x.Username == this.User)
                               .Include(x => x.Orders)
                               .FirstOrDefault();

            if (user == null)
            {
                return this.BadRequestError("You need to login first!", "Account/Login");
            }

            this.Model["username"] = user.Username;
            this.Model["registredOn"] = user.DateOfRegistration.ToString();
            this.Model["ordersCount"] = user.Orders.Count.ToString();

            return this.View();
        }
    }
}