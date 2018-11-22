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
        private ILogger logger;

        public EventsController(IEventService eventService, ILogger logger)
        {
            this.eventService = eventService;
            this.logger = logger;
        }

        [Authorize]
        public IActionResult All()
        {
            var events = this.eventService.GetAllEvents();

            var evetsViewModel = events.Select(x => new EventViewModel
                                               {
                                                  Name = x.Name,
                                                  Start = x.Start.ToShortDateString(),
                                                  End = x.End.ToShortDateString(),
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