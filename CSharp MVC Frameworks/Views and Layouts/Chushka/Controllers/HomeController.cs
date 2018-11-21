using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Chushka.Models;
using Chushka.Models.ViewModels.Home;
using Chushka.Services.Contracts;

namespace Chushka.Controllers
{
    public class HomeController : Controller
    {
        private IProductService productsService;

        public HomeController(IProductService productsService)
        {
            this.productsService = productsService;
        }

        public IActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.View();
            }

            var products = this.productsService.GetProducts();

            var productsViewModel = products.Select(x => new IndexProductViewModel
                                                         {
                                                             Id = x.Id,
                                                             Name = x.Name,
                                                             Description = x.Description,
                                                             Price = x.Price,
                                                             ShortDescription = x.Description?.Length > 50 ?
                                                                                x.Description?.Substring(0, 50) : x.Description
                                                         }).ToArray();

            return this.View(productsViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}