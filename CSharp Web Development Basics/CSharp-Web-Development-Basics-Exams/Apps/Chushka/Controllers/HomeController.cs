using Chushka.ViewModels.Home;
using SIS.HTTP.Responses;
using System.Linq;

namespace Chushka.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (!this.User.IsLoggedIn)
            {
                return this.View();
            }

            var products = this.db.Products.Where(x => x.IsDeleted == false)
                                           .Select(x => new IndexProductViewModel
                                                        {
                                                            Id = x.Id,
                                                            Name = x.Name,
                                                            Description = x.Description,
                                                            Price = x.Price,
                                                            ShortDescription = x.Description.Length > 50 ?
                                                                               x.Description.Substring(0, 50) : x.Description
                                                        }).ToArray();



            return this.View("/Home/IndexLoggedIn", products);
        }
    }
}