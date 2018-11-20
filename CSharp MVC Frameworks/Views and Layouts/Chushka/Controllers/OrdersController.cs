using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Services.Contracts;
using Models.ViewModels.Orders;

namespace Chushka.Controllers
{
    public class OrdersController : BaseController
    {
        private IOrderService ordersService;
        private IProductService productsService;

        public OrdersController(IOrderService ordersService, IProductService productsService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult All()
        {
            var orders = ordersService.GetAllOrders();

            var ordersViewModel = orders.Select(x => new OrderViewModel
                                {
                                    Id = x.Id,
                                    Client = x.Client.UserName,
                                    Product = x.Product.Name,
                                    OrderedOn = x.OrderedOn.ToShortDateString()
                                }).ToArray();

            return View(ordersViewModel);
        }

        [Authorize]
        public IActionResult Create(int id)
        {
            var product = productsService.GetProduct(id);

            if (product == null)
            {
                return this.Redirect("/");
            }

            var user = this.User.Identity.Name;

            ordersService.CreateOrder(product, user);

            return this.Redirect("/");
        }
    }
}