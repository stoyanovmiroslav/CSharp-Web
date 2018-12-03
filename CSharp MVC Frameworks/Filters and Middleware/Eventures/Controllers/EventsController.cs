using Eventures.Models.BindingModels;
using Eventures.Models.ViewModel;
using Eventures.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System.Linq;
using Eventures.Filters;
using AutoMapper;
using Eventures.Models;
using System.Collections.Generic;

namespace Eventures.Controllers
{
    public class EventsController : Controller
    {
        private IEventService eventService;
        private IOrderService orderService;
        private ILogger logger;
        private IMapper autoMapper;

        public EventsController(IEventService eventService, IOrderService orderService,
                                ILogger logger, IMapper mapper)
        {
            this.eventService = eventService;
            this.logger = logger;
            this.orderService = orderService;
            this.autoMapper = mapper;
        }

        [Authorize]
        public IActionResult My()
        {
            var username = this.User.Identity.Name;

            var orders = this.orderService.GetMyOrders(username);
            var evetsViewModel = autoMapper.Map<IList<Order>, IList<OrderEventViewModel>>(orders);

            return View(evetsViewModel);
        }

        [Authorize]
        public IActionResult All()
        {
            if (this.TempData.ContainsKey("error"))
            {
                ModelState.AddModelError(string.Empty, (string)this.TempData["error"]);
            }

            var events = this.eventService.GetAllAvailableEvents();
            var evetsViewModel = autoMapper.Map<IList<Event>, IList<EventViewModel>>(events);

            return View(evetsViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(LoggerActionFilter))]
        public IActionResult Create(CreateEvetBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            this.eventService.Create(model);

            return this.RedirectToAction("All");
        }
    }
}