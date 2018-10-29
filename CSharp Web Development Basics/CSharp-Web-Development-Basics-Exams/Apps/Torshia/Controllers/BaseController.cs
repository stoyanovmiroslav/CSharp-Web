using SIS.MvcFramework;
using Torshia.Data;

namespace Torshia.Controllers
{
    public class BaseController : Controller
    {
        protected TorshiaDbContext db;

        public BaseController()
        {
            this.db = new TorshiaDbContext();
        }
    }
}