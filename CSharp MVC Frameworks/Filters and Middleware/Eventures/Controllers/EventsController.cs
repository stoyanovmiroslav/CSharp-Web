using Eventures.Models.BindingModels;
using Eventures.Models.ViewModel;
using Eventures.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System.Linq;
using Eventures.Filters;

namespace Eventures.Controllers
{
    public class EventsController : Controller
    {
        private IEventService eventService;
        private IOrderService orderService;
        private ILogger logger;

        public EventsController(IEventService eventService, 
                                IOrderService orderService,
                                ILogger logger)
        {
            this.eventService = eventService;
            this.logger = logger;
            this.orderService = orderService;
        }

        [Authorize]
        public IActionResult My()
        {
            var username = this.User.Identity.Name;

            var oreders = this.orderService.GetMyOrders(username);

            var evetsViewModel = oreders.Select(x => new EventViewModel
            {
                Name = x.Event.Name,
                Start = x.Event.Start.ToString(),
                End = x.Event.End.ToString(),
                Place = x.Event.Place,
                TicketsCount = x.TicketsCount
            }).ToArray();

            return View(evetsViewModel);
        }

        [Authorize]
        public IActionResult All()
        {
            var events = this.eventService.GetAllEvents();

            var evetsViewModel = events.Select(x => new EventViewModel
                                               {
                                                  Id = x.Id,
                                                  Name = x.Name,
                                                  Start = x.Start.ToString(),
                                                  End = x.End.ToString(),
                                                  Place = x.Place,
                                               }).ToArray();

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