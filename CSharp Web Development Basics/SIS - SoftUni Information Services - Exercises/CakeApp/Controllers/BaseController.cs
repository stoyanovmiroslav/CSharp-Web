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
        }

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
    }
}