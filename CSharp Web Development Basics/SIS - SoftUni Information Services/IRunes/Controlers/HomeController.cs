using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.HttpAttributes;

namespace IRunes.Controlers
{
    public class HomeController : BaseController
    {
        [HttpGet("/")]
        public IHttpResponse Index()
        {
            this.ViewBag["username"] = this.User;

            return this.View();
        }
    }
}