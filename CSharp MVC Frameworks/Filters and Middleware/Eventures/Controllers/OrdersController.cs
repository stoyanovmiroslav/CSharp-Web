using AutoMapper;
using Eventures.Models;
using Eventures.Models.ViewModel;
using Eventures.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Eventures.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService ordereService;
        private readonly IEventService eventService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService ordereService, IEventService eventService, IMapper mapper)
        {
            this.ordereService = ordereService;
            this.eventService = eventService;
            this.mapper = mapper;
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
            var viewModel = mapper.Map<IList<Order>, IList<AllOrderViewModel>>(orders);

            return View(viewModel);
        }
    }
}