using Eventures.Models;
using Eventures.Models.ViewModel;
using Eventures.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventures.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private IOrderService ordereService;
        private IEventService eventService;

        public OrdersController(IOrderService ordereService, IEventService eventService)
        {
            this.ordereService = ordereService;
            this.eventService = eventService;
        }

        public IActionResult Create(string id, int ticketsCount)
        {
            int availableTickets = eventService.GetAvailableTickets(id);

            if (availableTickets < ticketsCount)
            {
                this.TempData["error"] = $"Sorry! We have only {availableTickets} tickets left for this event!";

                return RedirectToAction("All", "Events");
            }

            var username = this.User.Identity.Name;
            this.ordereService.CreateOrder(id, ticketsCount, username);

            return RedirectToAction("My", "Events");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult All()
        {
            var username = this.User.Identity.Name;

            var orders = this.ordereService.GetAllOrders();

            var viewModel = orders.Select(x => new AllOrderViewModel
            {
                Customer = x.Customer.UserName,
                Event = x.Event.Name,
                OrderedOn = x.OrderedOn.ToString()
            }).ToList();

            return View(viewModel);
        }
    }
}
