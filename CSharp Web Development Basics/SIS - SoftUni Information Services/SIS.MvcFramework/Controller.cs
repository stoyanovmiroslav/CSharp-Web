using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.Services;
using SIS.MvcFramework.Services.Contracts;
using SIS.WebServer.Results;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace SIS.MvcFramework
{
    public abstract class Controller
    {
        private const string VIEWS_FOLDER_PATH = "../../../Views";
        private const string FILE_EXTENTION = ".html";
        private const string DEFAULT_CONTROLER_NAME = "Controller";
        private const string LAYOUT = "_Layout";
        private const string ERROR_VIEW_PATH = "Error/Error";
        protected const string AUTH_COOKIE_KEY = "IRunes_auth";

        public Controller()
        {
            this.userCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();

            this.Response = new HttpResponse { StatusCode = HttpResponseStatusCode.Ok };
        }

        public IHttpRequest Request { get; set; }

        public IHttpResponse Response { get; set; }

        private string GetCurrentControllerName => this.GetType().Name.Replace(DEFAULT_CONTROLER_NAME, string.Empty);

        protected bool IsUserAuthenticated { get; set; } = false;

        protected IUserCookieService userCookieService { get; }

        protected Dictionary<string, string> ViewBag { get; set; }

        protected string User
        {
            get
            {
                if (!this.Request.Cookies.ContainsCookie(AUTH_COOKIE_KEY))
                {
                    return null;
                }

                this.IsUserAuthenticated = true;

                var cookie = this.Request.Cookies.GetCookie(AUTH_COOKIE_KEY);
                var cookieContent = cookie.Value;
                var userName = this.userCookieService.GetUserData(cookieContent);
                return userName;
            }
        }

        protected IHttpResponse View(string viewName = null)
        {
            string bodyContent = GetViewContent(viewName);

            this.SetViewBagParameters(bodyContent);

            string fullViewContent = GetViewContent(LAYOUT);

            PrepateHtmlResult(fullViewContent);

            return this.Response;
        }

        private void PrepateHtmlResult(string fullViewContent)
        {
            this.Response.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Response.Content = Encoding.UTF8.GetBytes(fullViewContent);
        }

        protected IHttpResponse Redirect(string location)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.Location, location));
            this.Response.StatusCode = HttpResponseStatusCode.SeeOther; // TODO: Found better?
            return this.Response;
        }

        protected IHttpResponse File(byte[] content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Response.Content = content;
            return this.Response;
        }

        protected IHttpResponse Text(string content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/plain; charset=utf-8"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
            return this.Response;
        }

        protected IHttpResponse BadRequestError(string massage = "Page Not Found", string currentViewPath = "Home/Index")
        {
            this.ViewBag["errorMassage"] = massage;

            StringBuilder bodyContent = new StringBuilder();
            bodyContent.Append(GetViewContent(ERROR_VIEW_PATH));
            bodyContent.Append(GetViewContent(currentViewPath));

            this.SetViewBagParameters(bodyContent.ToString());

            string fullViewContent = GetViewContent(LAYOUT);

            this.PrepateHtmlResult(fullViewContent);
            
            this.Response.StatusCode = HttpResponseStatusCode.BadRequest;

            return this.Response;
        }

        private void SetViewBagParameters(string bodyContent)
        {
            this.ViewBag["body"] = bodyContent;
            this.ViewBag["NonAuthenticated"] = "";
            this.ViewBag["Authenticated"] = "";

            if (IsUserAuthenticated)
            {
                this.ViewBag["NonAuthenticated"] = "d-none";
            }
            else
            {
                this.ViewBag["Authenticated"] = "d-none";
            }
        }

        protected string GetViewContent(string viewName)
        {
            string controlerName = GetCurrentControllerName;
            string actionName = new StackFrame(2).GetMethod().Name;

            string fullPath = $"{VIEWS_FOLDER_PATH}/{viewName}{FILE_EXTENTION}";

            if (viewName == null)
            {
                fullPath = $"{VIEWS_FOLDER_PATH}/{controlerName}/{actionName}{FILE_EXTENTION}";
            }

            var fileContent = System.IO.File.ReadAllText(fullPath);

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
    }
}