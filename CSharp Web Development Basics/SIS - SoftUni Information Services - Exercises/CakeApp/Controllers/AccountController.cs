using CakeApp.Data.Models;
using CakeApp.ViewModels.Account;
using SIS.Framework.ActionResult.Contracts;
using SIS.Framework.Attributes;
using SIS.Framework.Services.Contracts;
using SIS.HTTP.Cookies;
using System.Linq;

namespace CakeApp.Controllers
{
    public class AccountController : BaseController
    {
        IHashService hashService;

        public AccountController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Length < 6)
            {
                return this.BadRequestError("Username should be 6 or more characters long!");
            }

            if (this.db.Users.Any(x => x.Username == model.Username))
            {
                return this.BadRequestError("Username already exist!");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.BadRequestError("Confirm password does not match password!");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
            {
                return this.BadRequestError("Password should be 6 or more characters long!");
            }

            string hashedPassword = this.hashService.Hash(model.Password);

            User user = new User
            {
                Name = model.Username,
                Username = model.Username,
                Password = hashedPassword
            };

            db.Users.Add(user);
            db.SaveChanges();

            return this.View("login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                return this.BadRequestError("Invalid username or password!");
            }

            var hashedPassword = this.hashService.Hash(model.Password);

            User user = db.Users.FirstOrDefault(x => x.Username == model.Username && x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password!");
            }

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);
            this.Cookies.Add(new HttpCookie(AUTH_COOKIE_KEY, cookieContent));

            return this.RedirectToAction("/Home/Index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (!this.Request.Cookies.ContainsCookie(AUTH_COOKIE_KEY))
            {
                return this.RedirectToAction("/Home/Index");
            }

            var cookie = this.Request.Cookies.GetCookie(AUTH_COOKIE_KEY);
            cookie.Delete();

            this.Cookies.Add(cookie);

            return this.RedirectToAction("/Home/Index");
        }
    }
}