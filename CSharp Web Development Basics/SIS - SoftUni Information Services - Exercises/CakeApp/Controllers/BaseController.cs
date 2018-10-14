using CakeApp.Data;
using CakeApp.Services;
using CakeApp.Services.Contracts;
using SIS.Framework.ActionResult.Contracts;
using SIS.Framework.Controlers;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CakeApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected CakeAppDbContext db;
        
        protected BaseController()
        {
            this.db = new CakeAppDbContext();
            this.userCookieService = new UserCookieService();
        }

        public IUserCookieService userCookieService  { get; }


        //protected IActionResult ViewParameters(string viewName, string content)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(File.ReadAllText("Views/" + viewName + ".html"));
        //    sb.Append(content);

        //    return new HtmlResult(HttpResponseStatusCode.Ok, sb.ToString());
        //}

        //protected IActionResult BadRequestError(string errorMessage)
        //{
        //    var content = $"<h1>{errorMessage}</h1>";
        //    return new HtmlResult(HttpResponseStatusCode.BadRequest, content);
        //}


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