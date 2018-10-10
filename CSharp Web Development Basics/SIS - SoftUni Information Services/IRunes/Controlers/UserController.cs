using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRunes.Models;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.HttpAttributes;
using SIS.MvcFramework.Services;
using SIS.MvcFramework.Services.Contracts;
using SIS.WebServer.Results;

namespace IRunes.Controlers
{
    public class UserController : BaseController
    {
        IHashService hashService;

        public UserController()
        {
            this.hashService = new HashService();
        }

        [HttpGet("/user/login")]
        public IHttpResponse Login()
        {
            if (this.User != null)
            {
                this.ViewBag["username"] = this.User;
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost("/user/login")]
        public IHttpResponse LoginPost()
        {
            string username = this.Request.FormData["username"].ToString();
            string password = this.Request.FormData["password"].ToString();

            string hashedPassword = this.hashService.Hash(password);

            var user = db.Users.FirstOrDefault(x => x.Username == username && x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password!", "User/Login");
            }

            this.Request.Session.AddParameter("username", username);

            var userCookieValue = this.userCookieService.GetUserCookie(username);
            this.Response.Cookies.Add(new HttpCookie(AUTH_COOKIE_KEY, userCookieValue));

            return this.Redirect("/");
        }

        [HttpGet("/user/logout")]
        public IHttpResponse Logout()
        {
            if (this.User == null)
            {
                return this.Redirect("/");
            }

            var cookie = this.Request.Cookies.GetCookie(AUTH_COOKIE_KEY);
            cookie.Delete();

            this.Response.Cookies.Add(cookie);

            return this.Redirect("/");
        }

        [HttpGet("/user/register")]
        public IHttpResponse Register()
        {
            return this.View();
        }

        [HttpPost("/user/register")]
        public IHttpResponse RegisterPost()
        {
            string username = this.Request.FormData["username"].ToString();
            string password = this.Request.FormData["password"].ToString();
            string confirmPassword = this.Request.FormData["confirm-password"].ToString();
            string email = this.Request.FormData["email"].ToString();

            if (string.IsNullOrWhiteSpace(username) || username.Length < 6)
            {
                return this.BadRequestError("Username should be 6 or more characters long!", "User/Register");
            }

            if (this.db.Users.Any(x => x.Username == username))
            {
                return this.BadRequestError("Username already exist!", "User/Register");
            }

            if (password != confirmPassword)
            {
                return this.BadRequestError("Confirm password does not match password!", "User/Register");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return this.BadRequestError("Password should be 6 or more characters long!", "User/Register");
            }

            string hashedPassword = this.hashService.Hash(password);

            User user = new User
            {
                Username = username,
                Password = hashedPassword,
                Email = email
            };

            db.Users.Add(user);
            db.SaveChanges();

            var userCookieValue = this.userCookieService.GetUserCookie(username);
            this.Response.Cookies.Add(new HttpCookie(AUTH_COOKIE_KEY, userCookieValue));

            return this.Redirect("/");
        }
    }
}