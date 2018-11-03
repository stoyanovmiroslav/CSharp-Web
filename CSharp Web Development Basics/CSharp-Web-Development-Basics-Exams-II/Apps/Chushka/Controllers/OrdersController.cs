using Chushka.Models;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Linq;

namespace Chushka.Controllers
{
    public class OrdersController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse All()
        {
            var orders = this.db.Orders.Select(x => new OrderViewModel
            {
                Id = x.Id,
                Client = x.Client.Username,
                Product = x.Product.Name,
                OrderedOn = x.OrderedOn.ToShortDateString()
            }).ToArray();

            return this.View(orders);
        }

        [Authorize("Admin")]
        public IHttpResponse Create(int id)
        {
            var product = this.db.Products.Where(x => x.IsDeleted == false)
                                        .FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return this.BadRequestError("Invalid product!");
            }

            var user = this.db.Users.FirstOrDefault(x => x.Username == this.User.Username);

            if (user == null)
            {
                return this.BadRequestError("Invalid user!");
            }

            var order = new Order
            {
                Product = product,
                Client = user,
                OrderedOn = DateTime.UtcNow
            };

            this.db.Orders.Add(order);
            this.db.SaveChanges();

            return this.Redirect("/");
        }
    }
}