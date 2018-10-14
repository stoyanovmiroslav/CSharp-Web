using CakeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using SIS.Framework.ActionResult.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using System;
using System.Linq;
using System.Text;

namespace CakeApp.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            this.ViewBag["username"] = this.User;

            return this.View();
        }

        //public IActionResult Index(IHttpRequest request)
        //{
        //    var user = this.GetUsername(request);

        //    return this.View("Index", user);
        //}

        //    public IActionResult Profile(IHttpRequest request)
        //    {
        //        var usernameFromHash = this.GetUsername(request);

        //        if (usernameFromHash == null)
        //        {
        //            return this.BadRequestError("You need to login first!");
        //        }

        //        var user = db.Users.Where(x => x.Username == usernameFromHash)
        //                            .Include(x => x.Orders)
        //                            .FirstOrDefault();

        //        if (user == null)
        //        {
        //            return this.BadRequestError("You need to login first!");
        //        }

        //        string username = user.Username;
        //        string registredOn = user.DateOfRegistration.ToString();
        //        string ordersCount = user.Orders.Count.ToString();

        //        string content = $"<br/><b>My Profile</b><br/>Name: {username}<br/>Registered On: {registredOn}<br/>Orders Count: {ordersCount}";

        //        return this.ViewParameters("/profile", content);
        //    }


        //    public IActionResult AboutUs(IHttpRequest request)
        //    {
        //        return this.View("aboutus");
        //    }


        //    public IActionResult DoSearch(IHttpRequest request)
        //    {
        //        return this.BadRequestError("This page is under maintenance!");
        //    }


        //    public IActionResult Search(IHttpRequest request)
        //    {
        //        var cakes = db.Products.ToArray();

        //        if (cakes.Length < 1)
        //        {
        //            return BadRequestError("Sorry you have to add some cakes first!");
        //        }

        //        StringBuilder sb = new StringBuilder();

        //        foreach (var cake in cakes)
        //        {
        //            sb.AppendLine($"<a href=\"{cake.ImageUrl}\">{cake.Name}</a></li> ${cake.Price}<br/>");
        //        }

        //        return this.ViewParameters("search", sb.ToString());
        //    }
    }
}