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

        [Authorize]
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

        [Authorize]
        public IActionResult AboutUs()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Search(SearchViewModel model)
        {
            var cakes = db.Products.Where(x => x.Name == model.Search).ToArray();

            StringBuilder sb = new StringBuilder();

            int count = 1;
            foreach (var cake in cakes)
            {
                sb.AppendLine($"<tr><th scope =\"row\">{count++}</th>" +
                                $"<td><a href=\"/cake/cakeDetails?Id={cake.Id}\">{cake.Name}</a></td>" +
                                $"<td>${cake.Price}</td>" +
                                $"<td><button class=\"btn btn-primary\" type=\"submit\">Order</button></td></tr>");
            }

            if (cakes.Length == 0)
            {
                sb.AppendLine("<tr><th>-</th><th>Not found any cakes!</tr></th>");
            }

            this.Model["searchConrent"] = sb.ToString();

            return this.View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Search()
        {
            var cakes = db.Products.ToArray();

            if (cakes.Length < 1)
            {
                return BadRequestError("Sorry you have to add some cakes first!");
            }

            StringBuilder sb = new StringBuilder();

            int count = 1;
            foreach (var cake in cakes)
            {
                sb.AppendLine($"<tr><th scope =\"row\">{count++}</th>" +
                                  $"<td><a href=\"/cake/cakeDetails?Id={cake.Id}\">{cake.Name}</a></td>" +
                                  $"<td>${cake.Price}</td>" +
                                  $"<td><button class=\"btn btn-primary\" type=\"submit\">Order</button></td></tr>");
            }

            if (cakes.Length == 0)
            {
                sb.AppendLine("<tr><th>-</th><th>Not found any cakes!</tr></th>");
            }

            this.Model["searchConrent"] = sb.ToString();

            return this.View();
        }
    }
}