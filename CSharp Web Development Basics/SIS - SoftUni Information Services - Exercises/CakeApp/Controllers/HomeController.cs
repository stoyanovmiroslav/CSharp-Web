using CakeApp.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using SIS.Framework.ActionResult.Contracts;
using SIS.Framework.Attributes;
using System.Linq;
using System.Text;

namespace CakeApp.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            this.Model["username"] = this.User;

            return this.View();
        }

        public IActionResult Profile()
        {
            if (this.User == null)
            {
                return this.BadRequestError("You need to login first!");
            }

            var user = db.Users.Where(x => x.Username == this.User)
                                .Include(x => x.Orders)
                                .FirstOrDefault();

            if (user == null)
            {
                return this.BadRequestError("You need to login first!");
            }

            this.Model["username"] = user.Username;
            this.Model["registredOn"] = user.DateOfRegistration.ToString();
            this.Model["ordersCount"] = user.Orders.Count.ToString();

            return this.View();
        }

        public IActionResult AboutUs()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Search(SearchViewModel model)
        {
            return this.BadRequestError("This page is under maintenance!");
        }

        [HttpGet]
        public IActionResult Search()
        {
            var cakes = db.Products.ToArray();

            if (cakes.Length < 1)
            {
                return BadRequestError("Sorry you have to add some cakes first!");
            }

            StringBuilder sb = new StringBuilder();

            foreach (var cake in cakes)
            {
                sb.AppendLine($"<a href=\"{cake.ImageUrl}\">{cake.Name}</a> ${cake.Price}<br/>");
            }

            this.Model["searchConrent"] = sb.ToString();

            return this.View();
        }
    }
}