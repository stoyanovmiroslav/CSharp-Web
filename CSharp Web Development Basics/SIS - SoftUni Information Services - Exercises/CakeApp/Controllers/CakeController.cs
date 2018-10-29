using CakeApp.Data.Models;
using CakeApp.ViewModels.Cake;
using CakeApp.ViewModels.Home;
using SIS.Framework.ActionResult.Contracts;
using SIS.Framework.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CakeApp.Controllers
{
    public class CakeController : BaseController
    {
        [Authorize]
        [HttpGet]
        public IActionResult AddCake()
        {
            if (this.User == null)
            {
                return this.BadRequestError("You need to login first!", "Account/Login");
            }

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddCake(AddCakeViewModel model)
        {
            if (this.User == null)
            {
                return this.BadRequestError("you need to login first!", "Account/Login");
            }

            var user = db.Users.FirstOrDefault(x => x.Username == this.User);

            if (this.User == null)
            {
                return this.BadRequestError("you need to login first!", "Account/Login");
            }

            Order order = new Order()
            {
                DateOfCreation = DateTime.UtcNow,
                User = user
            };

            Product product = new Product
            {
                ImageUrl = WebUtility.UrlDecode(model.ImageUrl),
                Name = model.Name,
                Price = model.Price
            };

            OrderProduct orderproduct = new OrderProduct { Order = order, Product = product };

            this.db.OrderProducts.Add(orderproduct);
            db.SaveChanges();

            return this.RedirectToAction("/Home/Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult CakeDetails(int id)
        {
            var cake = db.Products.FirstOrDefault(x => x.Id == id);

            this.Model["Name"] = cake.Name; 
            this.Model["Price"] = cake.Price; 
            this.Model["Url"] = cake.ImageUrl;

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Search(SearchViewModel model)
        {
            var cakes = db.Products.Where(x => x.Name == model.Search).ToArray();

            if (cakes.Length == 0)
            {
                this.Model.Data["CakeViewModel"] = new List<SearchViewModel>()
                {
                    new SearchViewModel { Search = "Not found any cakes!" }
                };
            }
            else
            {
                this.Model.Data["CakeViewModel"] = cakes;
            }

            return this.View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Search()
        {
            var cakes = db.Products.ToArray();

            if (cakes.Length < 1)
            {
                return BadRequestError("Sorry you have to add some cakes first!", "Cake/AddCake");
            }

            if (cakes.Length == 0)
            {
                this.Model.Data["CakeViewModel"] = new List<SearchViewModel>()
                {
                    new SearchViewModel { Search = "Not found any cakes!" }
                };
            }
            else
            {
                this.Model.Data["CakeViewModel"] = cakes;
            }

            return this.View();
        }
    }
}