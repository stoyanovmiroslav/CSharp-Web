using Eventures.Data;
using Eventures.Models;
using Eventures.Models.BindingModels;
using Eventures.Services.Contracts;
using System;
using System.Collections.Generic;
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

        public IList<Event> GetAllAvailableEvents()
        {
           return this.db.Events.Where(x => x.TotalTickets > 0).ToList();
        }

        public int GetAvailableTickets(string id)
        {
            var @event = this.GetEventById(id);

            if (@event == null)
            {
                return 0;
            }

            return @event.TotalTickets;
        }

        private Event GetEventById(string id)
        {
            return this.db.Events.FirstOrDefault(x => x.Id == id);
        }
    }
}