using Chushka.Data;
using SIS.MvcFramework;

namespace Chushka.Controllers
{
    public class BaseController : Controller
    {
        protected ChushkaDbContext db;

        public BaseController()
        {
            this.db = new ChushkaDbContext();
        }
    }
}