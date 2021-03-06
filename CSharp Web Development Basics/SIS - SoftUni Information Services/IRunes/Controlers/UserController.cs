﻿using System.Linq;
using IRunes.Models;
using IRunes.ViewModels.User;
using SIS.HTTP.Cookies;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework;
using SIS.MvcFramework.HttpAttributes;
using SIS.MvcFramework.ViewEngine;
using SIS.MvcFramework.Services.Contracts;

namespace IRunes.Controlers
{
    public class UserController : BaseController
    {
        private readonly IHashService hashService;

        public UserController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet("/user/register")]
        public IHttpResponse Register()
        {
            if (this.User.Exist)
            {
                return this.BadRequestError("You have to logout first!", "/home/index");
            }

            return this.View();
        }

        [HttpPost("/user/register")]
        public IHttpResponse Register(RegisterViewModel model)
        {
            if (this.User.Exist)
            {
                return this.BadRequestError("You have to logout first!", "/home/index");
            }

            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Length < 6)
            {
                return this.BadRequestError("Username should be 6 or more characters long!", "user/register");
            }

            if (this.db.Users.Any(x => x.Username == model.Username))
            {
                return this.BadRequestError("Username already exist!", "user/register");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.BadRequestError("Confirm password does not match password!", "user/register");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
            {
                return this.BadRequestError("Password should be 6 or more characters long!", "user/register");
            }

            //Hash Password
            model.Password = this.hashService.Hash(model.Password);

            var user = model.To<User>();

            db.Users.Add(user);
            db.SaveChanges();

            SetSession(user, user.Username);
            AddCookieAuthentication(model.Username);

            return this.Redirect("/");
        }

        [HttpGet("/user/login")]
        public IHttpResponse Login()
        {
            if (this.User.Exist)
            {
                return this.BadRequestError("You are already login!", "/home/index");
            }

            return this.View();
        }

        [HttpPost("/user/login")]
        public IHttpResponse Login(LoginViewModel model)
        {
            if (this.User.Exist)
            {
                return this.BadRequestError("You are already login!", "/home/index");
            }

            string hashedPassword = this.hashService.Hash(model.Password);

            var user = db.Users.FirstOrDefault(x => x.Username == model.Username && x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password!", "user/login");
            }

            SetSession(user, user.Username);
            AddCookieAuthentication(model.Username);

            return this.Redirect("/");
        }

        [HttpGet("/user/logout")]
        public IHttpResponse Logout()
        {
            if (!this.User.Exist)
            {
                return this.BadRequestError("You are already logout!", "/home/index");
            }


            this.Request.Session.ClearParameters();
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

        private void SetSession(User user, string username)
        {
            var userModel = new UserModel { Name = user.Username, Exist = true };
            this.Request.Session.AddParameter(username, userModel);
        }
    }
}