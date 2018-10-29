using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Responses;
using System.Linq;

namespace MeTube.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (this.User.Username == null)
            {
                return this.View();
            }

            var user = Db.Users.Include(x => x.Tubes).FirstOrDefault(x => x.Username == this.User.Username);

            if (user == null)
            {
                this.BadRequestErrorWithView("Invalid user!");
            }

            var tubes = user.Tubes.ToArray();

            return this.View("Home/IndexLoggedIn", tubes);
        }
    }
}