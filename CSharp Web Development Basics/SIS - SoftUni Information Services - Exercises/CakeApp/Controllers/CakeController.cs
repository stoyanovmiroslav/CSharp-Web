using System;
using System.Collections.Generic;
using System.Text;

namespace CakeApp.Controllers
{
    public class CakeController
    {
        //    public IActionResult AddCake(IHttpRequest request)
        //    {
        //        var usernameFromHash = this.GetUsername(request);

        //        if (usernameFromHash == null)
        //        {
        //            return this.BadRequestError("You need to login first!");
        //        }

        //        return this.View("addcake");
        //    }

        //    public IActionResult DoAddCake(IHttpRequest request)
        //    {
        //        var usernameFromHash = this.GetUsername(request);

        //        if (usernameFromHash == null)
        //        {
        //            return this.BadRequestError("You need to login first!");
        //        }

        //        var user = db.Users.FirstOrDefault(x => x.Username == usernameFromHash);

        //        if (user == null)
        //        {
        //            return this.BadRequestError("You need to login first!");
        //        }

        //        string imageUrl = request.FormData["picture-url"].ToString();
        //        string name = request.FormData["name"].ToString();
        //        decimal price = decimal.Parse(request.FormData["price"].ToString());

        //        Order order = new Order()
        //        {
        //            DateOfCreation = DateTime.UtcNow,
        //            User = user
        //        };

        //        Product product = new Product
        //        {
        //            ImageUrl = imageUrl,
        //            Name = name,
        //            Price = price
        //        };

        //        OrderProduct orderProduct = new OrderProduct { Order = order, Product = product };

        //        this.db.OrderProducts.Add(orderProduct);
        //        db.SaveChanges();

        //        return this.View("Index");
        //    }
    }
}