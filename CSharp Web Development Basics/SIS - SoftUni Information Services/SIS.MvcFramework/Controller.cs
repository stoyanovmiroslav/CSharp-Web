using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.Services.Contracts;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SIS.MvcFramework
{
    public abstract class Controller
    {
        protected const string VIEWS_FOLDER_PATH = "../../../Views";

        protected const string HTML_EXTENTION = ".html";

        private const string DEFAULT_CONTROLER_NAME = "Controller";

        private const string LAYOUT = "_Layout";

        private const string ERROR_VIEW_PATH = "Error/Error";

        protected const string AUTH_COOKIE_KEY = "IRunes_auth";

        public Controller()
        {
            this.ViewBag = new Dictionary<string, string>();
            this.Response = new HttpResponse { StatusCode = HttpResponseStatusCode.Ok };
        }

        public IHttpRequest Request { get; set; }

        public IHttpResponse Response { get; set; }

        private string GetCurrentControllerName => this.GetType().Name.Replace(DEFAULT_CONTROLER_NAME, string.Empty);

        public IUserCookieService userCookieService { get; internal set; }

        protected Dictionary<string, string> ViewBag { get; set; }

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
                var userName = this.userCookieService.GetUserData(cookieContent);
                return userName;
            }
        }

        protected IHttpResponse View(string viewName = null)
        {
            string bodyContent = InsertViewParameters(viewName);

            this.SetViewBagParameters(bodyContent);

            string fullViewContent = InsertViewParameters(LAYOUT);

            PrepareHtmlResult(fullViewContent);

            return this.Response;
        }

        private void PrepareHtmlResult(string fullViewContent)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.CONTENT_TYPE, "text/html"));
            this.Response.Content = Encoding.UTF8.GetBytes(fullViewContent);
        }

        protected IHttpResponse Redirect(string location)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.LOCATION, location));
            this.Response.StatusCode = HttpResponseStatusCode.SeeOther;
            return this.Response;
        }

        protected IHttpResponse File(byte[] content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.CONTENT_LENGTH, content.Length.ToString()));
            this.Response.Headers.Add(new HttpHeader(HttpHeader.CONTENT_DISPOSITION, "inline"));
            this.Response.Content = content;
            return this.Response;
        }

        protected IHttpResponse Text(string content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.CONTENT_TYPE, "text/plain; charset=utf-8"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
            return this.Response;
        }

        protected IHttpResponse BadRequestError(string massage = "Page Not Found", string currentViewPath = "Home/Index")
        {
            this.ViewBag["errorMassage"] = massage;

            StringBuilder bodyContent = new StringBuilder();
            bodyContent.Append(InsertViewParameters(ERROR_VIEW_PATH));
            bodyContent.Append(InsertViewParameters(currentViewPath));

            this.SetViewBagParameters(bodyContent.ToString());

            string fullViewContent = InsertViewParameters(LAYOUT);

            this.PrepareHtmlResult(fullViewContent);
            
            this.Response.StatusCode = HttpResponseStatusCode.BadRequest;

            return this.Response;
        }

        private void SetViewBagParameters(string bodyContent)
        {
            this.ViewBag["body"] = bodyContent;
            this.ViewBag["NonAuthenticated"] = "";
            this.ViewBag["Authenticated"] = "";

            if (this.User == null)
            {
                this.ViewBag["Authenticated"] = "d-none";
            }
            else
            {
                this.ViewBag["NonAuthenticated"] = "d-none";
            }
        }

        protected string InsertViewParameters(string viewName)
        {
            string controlerName = GetCurrentControllerName;
            string actionName = new StackFrame(2).GetMethod().Name;

            string fullPath = $"{VIEWS_FOLDER_PATH}/{viewName}{HTML_EXTENTION}";

            if (viewName == null)
            {
                fullPath = $"{VIEWS_FOLDER_PATH}/{controlerName}/{actionName}{HTML_EXTENTION}";
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