using MishMash.Models;
using MishMash.ViewModels.Account;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework;
using SIS.MvcFramework.HttpAttributes;
using SIS.MvcFramework.ViewEngine;
using SIS.MvcFramework.Services.Contracts;
using System.Linq;

namespace MishMash.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IHashService hashService;

        public AccountController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet("/account/register")]
        public IHttpResponse Register()
        {
            if (this.User.Exist)
            {
                return this.BadRequestError("You have to logout first!");
            }

            return this.View();
        }

        [HttpPost("/account/register")]
        public IHttpResponse Register(RegisterViewModel model)
        {
            if (this.User.Exist)
            {
                return this.BadRequestError("You have to logout first!");
            }

            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Length < 6)
            {
                return this.BadRequestError("Username should be 6 or more characters long!", "account/register");
            }

            if (this.db.Users.Any(x => x.Username == model.Username))
            {
                return this.BadRequestError("Username already exist!", "account/register");
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

            model.Password = this.hashService.Hash(model.Password);

            var user = model.To<User>();

            db.Users.Add(user);
            db.SaveChanges();

            CreateSession(user);

            return this.Redirect("/");
        }

        [HttpGet("/account/login")]
        public IHttpResponse Login()
        {
            if (this.User.Exist)
            {
                return this.BadRequestError("You are already login!");
            }

            return this.View();
        }

        [HttpPost("/account/login")]
        public IHttpResponse Login(LoginViewModel model)
        {
            if (this.User.Exist)
            {
                return this.BadRequestError("You are already login!");
            }

            string hashedPassword = this.hashService.Hash(model.Password);

            var user = db.Users.FirstOrDefault(x => x.Username == model.Username && x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password!", "account/login");
            }

            CreateSession(user);

            return this.Redirect("/");
        }

        [HttpGet("/account/logout")]
        public IHttpResponse Logout()
        {
            if (!this.User.Exist)
            {
                return this.BadRequestError("You are already logout!", "/home/index-guest");
            }

            ClearSession();

            return this.Redirect("/");
        }

        private void ClearSession()
        {
            this.Request.Session.ClearParameters();
        }

        private void CreateSession(User user)
        {
            var userModel = new UserModel { Name = user.Username, Role = user.Role.ToString(), Email = user.Email };
            this.Request.Session.AddParameter(SESSION_KEY_INFO, userModel);
        }
    }
}