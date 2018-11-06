using SIS.HTTP.Responses;

namespace FluffyCats.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (this.User.Username == null)
            {
                return this.View();
            }

            return this.View("/Home/LoggedInIndex");
        }
    }
}