using SIS.CakeApp.Data.Models;
using SIS.CakeApp.Services;
using SIS.CakeApp.Services.Contracts;
using SIS.HTTP.Cookies;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIS.CakeApp.Controlers
{
    public class AccountController : BaseControler
    {
        IHashService hashService;

        public AccountController()
        {
            this.hashService = new HashService();
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View("Register");
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            return this.View("login");
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            string username = request.FormData["username"].ToString();
            string password = request.FormData["password"].ToString();
            string confirmPassword = request.FormData["confirm-password"].ToString();

            if (string.IsNullOrWhiteSpace(username) || username.Length < 6)
            {
                return this.BadRequestError("Username should be 6 or more characters long!");
            }

            if (this.db.Users.Any(x => x.Username == username))
            {
                return this.BadRequestError("Username already exist!");
            }

            if (password != confirmPassword)
            {
                return this.BadRequestError("confirm password does not match password!");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return this.BadRequestError("Password should be 6 or more characters long!");
            }

            string hashedPassword = this.hashService.Hash(password);

            User user = new User
            {
                Name = username,
                Username = username,
                Password = hashedPassword
            };

            db.Users.Add(user);
            db.SaveChanges();

            return this.View("login");
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            string username = request.FormData["username"].ToString();
            string password = request.FormData["password"].ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return this.BadRequestError("Invalid username or password!");
            }

            var hashedPassword = this.hashService.Hash(password);

            User user = db.Users.FirstOrDefault(x => x.Username == username && x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password!");
            }

            var cookieContent = this.userCookieService.GetUserCookie(user.Username);

            var response = new RedirectResult("/");
            var cookie = new HttpCookie(".auth-cakes", cookieContent) { HttpOnly = true };
            response.Cookies.Add(cookie);
            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return new RedirectResult("/");
            }

            var cookie = request.Cookies.GetCookie(".auth-cakes");
            cookie.Delete();
            var response = new RedirectResult("/");
            response.Cookies.Add(cookie);
            return response;
        }
    }
}