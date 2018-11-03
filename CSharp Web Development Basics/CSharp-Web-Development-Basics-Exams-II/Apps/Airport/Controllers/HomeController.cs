using Airport.Models;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System.Linq;

namespace Airport.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (!this.User.IsLoggedIn)
            {
                return this.View();
            }

            return this.View();
        }

        public IHttpResponse Cart()
        {
            return this.View();
        }

        [Authorize]
        public IHttpResponse Flights()
        {
            Fligh[] flight = null;

            if (this.User.Role == "Admin")
            {
                flight = this.db.Flighs.ToArray();
            }
            else
            {
                flight = this.db.Flighs.Where(x => x.PulicFlag == true).ToArray();
            }

            return this.View(flight);
        }
    }
}