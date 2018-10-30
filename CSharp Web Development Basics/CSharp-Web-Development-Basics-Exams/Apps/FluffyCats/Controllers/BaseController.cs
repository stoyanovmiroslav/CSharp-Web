using FluffyCats.Data;
using SIS.MvcFramework;


namespace FluffyCats.Controllers
{
    public class BaseController : Controller
    {
        protected FluffyCatsDbContext db;

        public BaseController()
        {
            this.db = new FluffyCatsDbContext();
        }
    }
}