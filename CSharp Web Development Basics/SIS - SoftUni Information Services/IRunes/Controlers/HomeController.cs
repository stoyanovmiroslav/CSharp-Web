using IRunes.ViewModels.Home;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.HttpAttributes;

namespace IRunes.Controlers
{
    public class HomeController : BaseController
    {
        [HttpGet("/")]
        public IHttpResponse Index()
        {
            IndexViewModel model = new IndexViewModel { Username = this.User };

            return this.View(model);
        }
    }
}