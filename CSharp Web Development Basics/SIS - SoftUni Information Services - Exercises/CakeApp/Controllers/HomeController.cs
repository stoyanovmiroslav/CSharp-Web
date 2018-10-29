using SIS.Framework.ActionResult.Contracts;
using SIS.Framework.Attributes;

namespace CakeApp.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            this.Model["username"] = this.User;

            return this.View();
        }

        [Authorize]
        public IActionResult AboutUs()
        {
            return this.View();
        }
    }
}