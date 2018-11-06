using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Responses;
using System.Linq;

namespace Torshia.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (this.User.Username == null)
            {
                return this.View("/Home/GuestIndex");
            }

            var task = db.Tasks.Include(x => x.AffectedSectors).Where(x => x.IsReported == false).ToArray();

            return this.View(task);
        }
    }
}