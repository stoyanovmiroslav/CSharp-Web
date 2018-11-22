using Eventures.Data;
using Eventures.Models;
using Eventures.Models.BindingModels;
using Eventures.Services.Contracts;
using System.Linq;

namespace Eventures.Services
{
    public class EventService : IEventService
    {
        private EventuresDbContext db;

        public EventService(EventuresDbContext db)
        {
            this.db = db;
        }

        public void Create(CreateEvetBindingModel model)
        {
            var @event = new Event
            {
                Name = model.Name,
                PricePerTicket = model.PricePerTicket,
                Start = model.Start,
                End = model.End,
                Place = model.Place,
                TotalTickets = model.TotalTickets
            };

            this.db.Events.Add(@event);
            this.db.SaveChanges();
        }

        public Event[] GetAllEvents()
        {
           return  this.db.Events.ToArray();
        }
    }
}