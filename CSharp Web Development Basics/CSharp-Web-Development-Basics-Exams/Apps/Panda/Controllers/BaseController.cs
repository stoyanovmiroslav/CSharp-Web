using Panda.Data;
using SIS.MvcFramework;

namespace Panda.Controllers
{
    public class BaseController : Controller
    {
        protected PandaDbContext db;

        public BaseController()
        {
            this.db = new PandaDbContext();
        }
    }
}