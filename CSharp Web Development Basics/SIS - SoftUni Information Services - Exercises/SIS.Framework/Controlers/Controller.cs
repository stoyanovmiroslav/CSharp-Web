using SIS.Framework.ActionResult;
using SIS.Framework.ActionResult.Contracts;
using SIS.Framework.Models;
using SIS.Framework.Security.Contracts;
using SIS.Framework.Services;
using SIS.Framework.Utilities;
using SIS.Framework.Views;
using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Requests.Contracts;
using System;
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

        private ViewEngine ViewEngine { get; } = new ViewEngine();

        public IUserCookieService UserCookieService { get; set; }

        public IHttpRequest Request { get; set; }

        public Model ModelState { get; } = new Model();

        public ViewModel Model { get; set; }

        public IHttpCookieCollection Cookies { get; set; }

        public IIdentity Identity()
        {
            if (this.Request.Session.ContainsParameter(AUTH_COOKIE_KEY))
            {
                return (IIdentity)this.Request.Session.GetParameter(AUTH_COOKIE_KEY);
            }

            return null;
        }

        protected void SingIn(IIdentity identity)
        {
            this.Request.Session.AddParameter(AUTH_COOKIE_KEY, identity);
        }

        protected void SingOut(IIdentity identity)
        {
            this.Request.Session.ClearParameters();
        }

        protected IViewable View([CallerMemberName] string viewName = "")
        {
            var controllerName = ControllerUtilities.GetContorllerName(this);
            string viewContent = null;
            SetDisplayTags();

            try
            {
                viewContent = this.ViewEngine.GetViewContent(controllerName, viewName);
            }
            catch (Exception e)
            {
                this.Model.Data["errorMassage"] = e.Message;

                viewContent = this.ViewEngine.GetErrorContent();
            }

            string renderedHtml = this.ViewEngine.RenderHtml(viewContent, this.Model.Data);
            var view = new View(renderedHtml);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);

        protected IViewable BadRequestError(string massage = "Invalid Operation!", string viewBodyPath = null)
        {
            this.Model["errorMassage"] = massage;

            SetDisplayTags();

            var viewContent = this.ViewEngine.GetErrorContent(viewBodyPath);

            string renderedHtml = this.ViewEngine.RenderHtml(viewContent, this.Model.Data);
            var view = new View(renderedHtml);

            return new ViewResult(view);
        }

        private void SetDisplayTags()
        {
            this.Model["NonAuthenticated"] = this.User != null ? "d-none" : "";
            this.Model["Authenticated"] = this.User == null ? "d-none" : "";
        }

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