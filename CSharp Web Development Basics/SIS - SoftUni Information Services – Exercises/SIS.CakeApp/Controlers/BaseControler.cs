using SIS.CakeApp.Data;
using SIS.CakeApp.Services;
using SIS.CakeApp.Services.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIS.CakeApp.Controlers
{
    public abstract class BaseControler
    {
        protected CakeAppDbContext db;

        protected BaseControler()
        {
            this.db = new CakeAppDbContext();
            this.userCookieService = new UserCookieService();
        }

        public IUserCookieService userCookieService  { get; }

        protected IHttpResponse View(string viewName)
        {
            var content = File.ReadAllText("Views/" + viewName + ".html");
            return new HtmlResult(HttpResponseStatusCode.Ok, content);
        }

        protected IHttpResponse View(string viewName, string name)
        {
            StringBuilder sb = new StringBuilder();

            if (name != null)
            {
                sb.Append($"<H1>Hello {name}!<H1>");
            }

            sb.Append(File.ReadAllText("Views/" + viewName + ".html"));

            return new HtmlResult(HttpResponseStatusCode.Ok, sb.ToString());
        }

        protected IHttpResponse ViewParameters(string viewName, string content)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(File.ReadAllText("Views/" + viewName + ".html"));
            sb.Append(content);

            return new HtmlResult(HttpResponseStatusCode.Ok, sb.ToString());
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            var content = $"<h1>{errorMessage}</h1>";
            return new HtmlResult(HttpResponseStatusCode.BadRequest, content);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult(HttpResponseStatusCode.InternalServerError, $"<h1>{errorMessage}</h1>");
        }

        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return null;
            }
            var cookie = request.Cookies.GetCookie(".auth-cakes");
            var cookieContent = cookie.Value;
            var userName = this.userCookieService.GetUserData(cookieContent);
            return userName;
        }
    }
}