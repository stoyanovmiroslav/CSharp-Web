using SIS.Framework.ActionResult;
using SIS.Framework.ActionResult.Contracts;
using SIS.Framework.Models;
using SIS.Framework.Services;
using SIS.Framework.Utilities;
using SIS.Framework.Views;
using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Requests.Contracts;
using System.Runtime.CompilerServices;

namespace SIS.Framework.Controlers
{
    public abstract class Controller
    {
        protected const string AUTH_COOKIE_KEY = ".auth_cake";

        public Controller()
        {
            this.Model = new ViewModel();
            this.Cookies = new HttpCookieCollection();
            this.UserCookieService = new UserCookieService();
        }

        public IUserCookieService UserCookieService { get; set; }

        public IHttpRequest Request { get; set; }

        public Model ModelState { get; } = new Model();

        public ViewModel Model { get; set; }

        public IHttpCookieCollection Cookies { get; set; }

        protected IViewable View([CallerMemberName] string caller = "")
        {
            var controllerName = ControllerUtilities.GetContorlerName(this);
            var fullQualifiedName = ControllerUtilities.GetFullQualifiedName(controllerName, caller);

            SetViewBagParameters();

            var view = new View(fullQualifiedName, this.Model.Data);

            return new ViewResult(view);
        }

        protected IViewable BadRequestError(string massage = "Invalid Operation!", string viewName = "Error")
        {
            this.Model["errorMassage"] = massage;

            SetViewBagParameters();

            string errorHtmlPath = $"{MvcContext.Get.ViewFolderFullPath}/{MvcContext.Get.ErrorViewFolder}/{viewName}.{MvcContext.Get.HtmlFileExtention}";

            var view = new View(errorHtmlPath, this.Model.Data);

            return new ViewResult(view);
        }

        private void SetViewBagParameters()
        {
            this.Model["NonAuthenticated"] = "";
            this.Model["Authenticated"] = "";

            if (this.User == null)
            {
                this.Model["Authenticated"] = "d-none";
            }
            else
            {
                this.Model["NonAuthenticated"] = "d-none";
            }
        }

        protected IRedirectable RedirectToAction(string redirectUrl) 
            => new RedirectResult(redirectUrl);

        protected string User
        {
            get
            {
                if (!this.Request.Cookies.ContainsCookie(AUTH_COOKIE_KEY))
                {
                    return null;
                }

                var cookie = this.Request.Cookies.GetCookie(AUTH_COOKIE_KEY);
                var cookieContent = cookie.Value;
                var userName = this.UserCookieService.GetUserData(cookieContent);
                return userName;
            }
        }
    }
}