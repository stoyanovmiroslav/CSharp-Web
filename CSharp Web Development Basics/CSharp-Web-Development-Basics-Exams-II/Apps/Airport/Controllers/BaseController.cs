using Airport.Data;
using SIS.MvcFramework;

namespace Airport.Controllers
{
    public class BaseController : Controller
    {
        protected AirportDbContext db;

        public BaseController()
        {
            this.db = new AirportDbContext();
        }
    }
}