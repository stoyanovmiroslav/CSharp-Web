using CakeApp.Data;
using SIS.Framework.Controlers;

namespace CakeApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected CakeAppDbContext db;
        
        protected BaseController()
        {
            this.db = new CakeAppDbContext();
        }
    }
}