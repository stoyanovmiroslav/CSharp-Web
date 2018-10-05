using IRunes.Data;
using IRunes.Services;
using IRunes.Services.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IRunes.Controlers
{
    public abstract class BaseController
    {
        protected IRunesDbContext db;

        private const string Root = "../../../Views/";
        private const string FileExtention = ".html";

        protected BaseController()
        {
            this.db = new IRunesDbContext();
            this.userCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        public Dictionary<string, string> ViewBag { get; set; }

        public IUserCookieService userCookieService { get; }

        protected IHttpResponse View(string viewName)
        {
            string content = ViewFactory(viewName);

            this.ViewBag["body"] = content;

            viewName = "_LayoutLogin";

            string fullcontent = ViewFactory(viewName);


            return new HtmlResult(HttpResponseStatusCode.Ok, fullcontent);
        }

        protected string ViewFactory(string viewName)
        {
            var content = File.ReadAllText(Root + viewName + FileExtention);

            foreach (var viewBagKey in ViewBag.Keys)
            {
                string placeHolder = $"{{{{{viewBagKey}}}}}";

                if (content.Contains(viewBagKey))
                {
                    content = content.Replace(placeHolder, this.ViewBag[viewBagKey]);
                }
            }

            return content;
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            var content = $"<h1>{errorMessage}</h1>";
            return new HtmlResult(HttpResponseStatusCode.BadRequest, content);
        }

        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie("IRunes_auth"))
            {
                return null;
            }
            var cookie = request.Cookies.GetCookie("IRunes_auth");
            var cookieContent = cookie.Value;
            var userName = this.userCookieService.GetUserData(cookieContent);
            return userName;
        }
    }
}