using System.Linq;
using IRunes.Models;
using IRunes.ViewModels.User;
using SIS.HTTP.Cookies;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.HttpAttributes;
using SIS.MvcFramework.Services.Contracts;

namespace IRunes.Controlers
{
    public class UserController : BaseController
    {
        IHashService hashService;

        public UserController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet("/user/login")]
        public IHttpResponse Login()
        {
            if (this.User != null)
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost("/user/login")]
        public IHttpResponse Login(LoginViewModel model)
        {
            string hashedPassword = this.hashService.Hash(model.Password);

            var user = db.Users.FirstOrDefault(x => x.Username == model.Username && x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password!", "User/Login");
            }

            this.Request.Session.AddParameter("username", model.Username);

            var userCookieValue = this.UserCookieService.GetUserCookie(model.Username);
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
        public IHttpResponse DoRegister(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Length < 6)
            {
                return this.BadRequestError("Username should be 6 or more characters long!", "/User/Register");
            }

            if (this.db.Users.Any(x => x.Username == model.Username))
            {
                return this.BadRequestError("Username already exist!", "/User/Register");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.BadRequestError("Confirm password does not match password!", "User/Register");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
            {
                return this.BadRequestError("Password should be 6 or more characters long!", "User/Register");
            }

            string hashedPassword = this.hashService.Hash(model.Password);

            User user = new User
            {
                Username = model.Username,
                Password = hashedPassword,
                Email = model.Email
            };

            db.Users.Add(user);
            db.SaveChanges();

            var userCookieValue = this.UserCookieService.GetUserCookie(model.Username);
            this.Response.Cookies.Add(new HttpCookie(AUTH_COOKIE_KEY, userCookieValue));

            return this.Redirect("/");
        }
    }
}