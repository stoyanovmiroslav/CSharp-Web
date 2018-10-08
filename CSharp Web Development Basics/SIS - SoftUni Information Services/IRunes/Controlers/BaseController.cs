using IRunes.Data;
using IRunes.Services;
using IRunes.Services.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace IRunes.Controlers
{
    public abstract class BaseController
    {
        private const string VIEW_FOLDER_PATH = "../../../Views";
        private const string FILE_EXTENTION = ".html";
        private const string DEFAULT_CONTROLER_NAME = "Controller";
        private const string LAYOUT = "_Layout";
        private const string ERROR_VIEW_PATH = "Error/Error";


        private string GetCurrentControllerName => this.GetType().Name.Replace(DEFAULT_CONTROLER_NAME, string.Empty);

        protected IRunesDbContext db;

        protected BaseController()
        {
            this.db = new IRunesDbContext();
            this.userCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        protected bool IsUserAuthenticated { get; set; } = false;

        protected IUserCookieService userCookieService { get; }

        protected Dictionary<string, string> ViewBag { get; set; }

        protected IHttpResponse View(string viewName = null)
        {
            string bodyContent = GetViewContent(viewName);

            this.SetLoyoutViewBag(bodyContent);

            string viewContent = GetViewContent(LAYOUT);

            return new HtmlResult(HttpResponseStatusCode.Ok, viewContent);
        }

        protected IHttpResponse BadRequestError(string massage = "Page Not Found", string currentView = "Home/Index")
        {
            this.ViewBag["errorMassage"] = massage;

            StringBuilder bodyContent = new StringBuilder();
            bodyContent.Append(GetViewContent(ERROR_VIEW_PATH));
            bodyContent.Append(GetViewContent(currentView));

            this.SetLoyoutViewBag(bodyContent.ToString());

            string fullViewContent = GetViewContent(LAYOUT);

            return new HtmlResult(HttpResponseStatusCode.BadRequest, fullViewContent);
        }

        private void SetLoyoutViewBag(string bodyContent)
        {
            this.ViewBag["body"] = bodyContent;
            this.ViewBag["isLoginNavVisable"] = "";
            this.ViewBag["isLogoutNavVisable"] = "";

            if (IsUserAuthenticated)
            {
                this.ViewBag["isLoginNavVisable"] = "d-none";
            }
            else
            {
                this.ViewBag["isLogoutNavVisable"] = "d-none";
            }
        }

        protected string GetViewContent(string viewName)
        {
            string controlerName = GetCurrentControllerName;
            string actionName = new StackFrame(2).GetMethod().Name;

            string fullPath = $"{VIEW_FOLDER_PATH}/{viewName}{FILE_EXTENTION}";

            if (viewName == null)
            {
                fullPath = $"{VIEW_FOLDER_PATH}/{controlerName}/{actionName}{FILE_EXTENTION}";
            }

            var fileContent = File.ReadAllText(fullPath);

            foreach (var viewBagKey in ViewBag.Keys)
            {
                string placeHolder = $"{{{{{viewBagKey}}}}}";

                if (fileContent.Contains(viewBagKey))
                {
                    fileContent = fileContent.Replace(placeHolder, this.ViewBag[viewBagKey]);
                }
            }

            return fileContent;
        }

        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie("IRunes_auth"))
            {
                return null;
            }

            this.IsUserAuthenticated = true;

            var cookie = request.Cookies.GetCookie("IRunes_auth");
            var cookieContent = cookie.Value;
            var userName = this.userCookieService.GetUserData(cookieContent);
            return userName;
        }
    }
}