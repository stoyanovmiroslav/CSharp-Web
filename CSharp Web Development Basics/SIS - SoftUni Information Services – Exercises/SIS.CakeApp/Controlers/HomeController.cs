using Microsoft.EntityFrameworkCore;
using SIS.CakeApp.Data.Models;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIS.CakeApp.Controlers
{
    public class HomeController : BaseControler
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            var user = this.GetUsername(request);

            return this.View("Index", user);
        }

        public IHttpResponse HelloUser(IHttpRequest request)    
        {
            return new HtmlResult(HttpResponseStatusCode.Ok, $"<h1>Hello, {this.GetUsername(request)}</h1>");
        }

        public IHttpResponse Profile(IHttpRequest request)
        {
            var usernameFromHash = this.GetUsername(request);

            if (usernameFromHash == null)
            {
                return this.BadRequestError("You need to login first!");
            }

            var user = db.Users.Where(x => x.Username == usernameFromHash)
                                .Include(x => x.Orders)
                                .FirstOrDefault();

            if (user == null)
            {
                return this.BadRequestError("You need to login first!");
            }

            string username = user.Username;
            string registredOn = user.DateOfRegistration.ToString();
            string ordersCount = user.Orders.Count.ToString();

            string content = $"<br/><b>My Profile</b><br/>Name: {username}<br/>Registered On: {registredOn}<br/>Orders Count: {ordersCount}";

            return this.ViewParameters("/profile", content);
        }

        public IHttpResponse AddCake(IHttpRequest request)
        {
            var usernameFromHash = this.GetUsername(request);

            if (usernameFromHash == null)
            {
                return this.BadRequestError("You need to login first!");
            }

            return this.View("addcake");
        }

        public IHttpResponse AboutUs(IHttpRequest request)
        {
            return this.View("aboutus");
        }

        public IHttpResponse DoSearch(IHttpRequest request)
        {
            return this.BadRequestError("This page is under maintenance!");
        }

        public IHttpResponse Search(IHttpRequest request)
        {
            var cakes = db.Products.ToArray();

            if (cakes.Length < 1)
            {
                return BadRequestError("Sorry you have to add some cakes first!");
            }

            StringBuilder sb = new StringBuilder();

            foreach (var cake in cakes)
            {
                sb.AppendLine($"<a href=\"{cake.ImageUrl}\">{cake.Name}</a></li> ${cake.Price}<br/>");
            }

            return this.ViewParameters("search", sb.ToString());
        }

        public IHttpResponse DoAddCake(IHttpRequest request)
        {
            var usernameFromHash = this.GetUsername(request);

            if (usernameFromHash == null)
            {
                return this.BadRequestError("You need to login first!");
            }

            var user = db.Users.FirstOrDefault(x => x.Username == usernameFromHash);

            if (user == null)
            {
                return this.BadRequestError("You need to login first!");
            }

            string imageUrl = request.FormData["picture-url"].ToString();
            string name = request.FormData["name"].ToString();
            decimal price = decimal.Parse(request.FormData["price"].ToString());

            Order order = new Order()
            {
                DateOfCreation = DateTime.UtcNow,
                User = user
            };

            Product product = new Product
            {
                ImageUrl = imageUrl,
                Name = name,
                Price = price
            };

            OrderProduct orderProduct = new OrderProduct { Order = order, Product = product };

            this.db.OrderProducts.Add(orderProduct);
            db.SaveChanges();

            return this.View("Index");
        }
    }
}