using System;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace IRunes.Controlers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
          //  this.ViewBag["NonAuthenticated"] = "";
          //  this.ViewBag["Authenticated"] = "";

            var username = this.GetUsername(request);
            if (username == null)
            {
               // this.ViewBag["Authenticated"] = "d-none";
                return this.View();
            }

           // this.ViewBag["NonAuthenticated"] = "d-none";
            this.ViewBag["username"] = username;

            return this.View();
        }
    }
}