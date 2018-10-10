using IRunes.Data;
using SIS.MvcFramework;

namespace IRunes.Controlers
{
    public abstract class BaseController : Controller
    {
        protected IRunesDbContext db;

        protected BaseController()
        {
            this.db = new IRunesDbContext();
        }
    }
}