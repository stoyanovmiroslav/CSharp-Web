using System;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.HttpAttributes;

namespace IRunes.Controlers
{
    public class HomeController : BaseController
    {
        [HttpGet("/")]
        public IHttpResponse Index()
        {
            if (this.User == null)
            {
                return this.View();
            }
          
            this.ViewBag["username"] = this.User;

            return this.View();
        }
    }
}