using MishMash.Models;
using MishMash.ViewModels.Account;
using SIS.HTTP.Cookies;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework;
using SIS.MvcFramework.HttpAttributes;
using SIS.MvcFramework.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MishMash.Controllers
{
    public class AccountController : BaseController
    {
        IHashService hashService;

        public AccountController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet("/account/register")]
        public IHttpResponse Register()
        {
            if (this.User != null)
            {
                return this.BadRequestError("You have to logout first!", "/home/index");
            }

            return this.View();
        }

        [HttpPost("/account/register")]
        public IHttpResponse Register(RegisterViewModel model)
        {
            if (this.User != null)
            {
                return this.BadRequestError("You have to logout first!", "/home/index");
            }

            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Length < 6)
            {
                return this.BadRequestError("Username should be 6 or more characters long!", "account/register");
            }

            if (this.db.Users.Any(x => x.Username == model.Username))
            {
                return this.BadRequestError("Username already exist!", "user/register");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.BadRequestError("Confirm password does not match password!", "account/register");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
            {
                return this.BadRequestError("Password should be 6 or more characters long!", "account/register");
            }

            model.Role = Models.Enums.Role.User;

            if (!db.Users.Any())
            {
                model.Role = Models.Enums.Role.Admin;
            }

            //Hash Password
            model.Password = this.hashService.Hash(model.Password);

            var user = model.To<User>();

            db.Users.Add(user);
            db.SaveChanges();

            this.Request.Session.AddParameter("username", model.Username);
            AddCookieAuthentication(model.Username);

            return this.Redirect("/");
        }

        [HttpGet("/account/login")]
        public IHttpResponse Login()
        {
            if (this.User != null)
            {
                return this.BadRequestError("You are already login!", "/home/index");
            }

            return this.View();
        }

        [HttpPost("/account/login")]
        public IHttpResponse Login(LoginViewModel model)
        {
            if (this.User != null)
            {
                return this.BadRequestError("You are already login!", "/home/index");
            }

            string hashedPassword = this.hashService.Hash(model.Password);

            var user = db.Users.FirstOrDefault(x => x.Username == model.Username && x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password!", "account/login");
            }

            this.Request.Session.AddParameter("username", model.Username);
            AddCookieAuthentication(model.Username);

            return this.Redirect("/");
        }

        [HttpGet("/account/logout")]
        public IHttpResponse Logout()
        {
            if (this.User == null)
            {
                return this.BadRequestError("You are already logout!", "/home/index-guest");
            }

            var cookie = this.Request.Cookies.GetCookie(AUTH_COOKIE_KEY);
            cookie.Delete();

            this.Response.Cookies.Add(cookie);

            return this.Redirect("/");
        }

        private void AddCookieAuthentication(string username)
        {
            var userCookieValue = this.UserCookieService.GetUserCookie(username);
            this.Response.Cookies.Add(new HttpCookie(AUTH_COOKIE_KEY, userCookieValue));
        }
    }
}