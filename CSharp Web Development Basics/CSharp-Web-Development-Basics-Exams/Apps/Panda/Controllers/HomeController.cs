using Microsoft.EntityFrameworkCore;
using Panda.Models.Enums;
using Panda.ViewModels.Home;
using SIS.HTTP.Responses;
using System.Linq;

namespace Panda.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (!this.User.IsLoggedIn)
            {
                return this.View();
            }

            var pending = this.db.Packages
                                 .Include(x => x.Recipient)
                                 .Include(x => x.Receipt)
                                 .Where(x => x.Recipient.Username == this.User.Username && x.Status == Status.Pending)
                                 .ToArray();

            var shipped = this.db.Packages
                                 .Include(x => x.Recipient)
                                 .Include(x => x.Receipt)
                                 .Where(x => x.Recipient.Username == this.User.Username && x.Status == Status.Shipped)
                                 .ToArray();

            var delivered = this.db.Packages
                                   .Include(x => x.Recipient)
                                   .Include(x => x.Receipt)
                                   .Where(x => x.Recipient.Username == this.User.Username && x.Status == Status.Delivered)
                                   .ToArray();


            var homeViewModel = new HomeViewModel
            {
                Pending = pending,
                Shipped = shipped,
                Delivered = delivered
            };

            return this.View("/Home/IndexLoggedIn", homeViewModel);
        }
    }
}