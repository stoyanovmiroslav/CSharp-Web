using SIS.Framework.ActionResult;
using SIS.Framework.ActionResult.Contracts;
using SIS.Framework.Services;
using SIS.Framework.Utilities;
using SIS.Framework.Views;
using SIS.HTTP.Requests.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SIS.Framework.Controlers
{
    public class Controller
    {
        protected const string AUTH_COOKIE_KEY = "IRunes_auth";

        public Controller()
        {
            this.UserCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        public IUserCookieService UserCookieService { get; set; }

        public IHttpRequest Request { get; set; }

        protected Dictionary<string, string> ViewBag { get; set; }

        protected IViewable View([CallerMemberName] string caller = "")
        {
            var controllerName = ControllerUtilities.GetContorlerName(this);

            var fullQualifiedName = ControllerUtilities.GetFullQualifiedName(controllerName, caller);

            var view = new View(fullQualifiedName, this.ViewBag);

            return new ViewResult(view);
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