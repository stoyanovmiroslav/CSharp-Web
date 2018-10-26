using MishMash.Data;
using SIS.MvcFramework;

namespace MishMash.Controllers
{
    public class BaseController : Controller
    {
        protected MishMashDbContext db;

        public BaseController()
        {
            db = new MishMashDbContext();
        }
    }
}