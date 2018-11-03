using Airport.Models;
using Airport.Tickets;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System.Globalization;
using System.Linq;

namespace Airport.Controllers
{
    public class TicketsController : BaseController
    {
        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Add(AddTicketsViewModel model)
        {
            var flight = this.db.Flighs.FirstOrDefault(x => x.Id == model.Id);

            if (flight == null)
            {
                return this.BadRequestError("Invalid flight!");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var ticket = new Ticket
                {
                    Fligh = flight,
                    Class = model.Class,
                    Price = model.Price,
                };

                flight.Tickets.Add(ticket);
            }

            this.db.SaveChanges();

            return this.Redirect("/Flights/Details?id=" + model.Id);
        }

        [Authorize("Admin")]
        public IHttpResponse All()
        {
            var allTicketsViewModel = this.db.Tickets
                                             .Where(x => x.Customer.Username == this.User.Username)
                                             .Select(x => new AllTicketsViewModel
                                             {
                                                 Destination = x.Fligh.Destination,
                                                 Origin = x.Fligh.Origin,
                                                 Class = x.Class,
                                                 Image = x.Fligh.Image,
                                                 Count = 1,
                                                 Price = x.Price,
                                                 Date = x.Fligh.Date.ToString("dd MMM", CultureInfo.InvariantCulture),
                                                 Time = x.Fligh.Date.ToString("HH:mm", CultureInfo.InvariantCulture)
                                             }).ToArray();

            return this.View(allTicketsViewModel);
        }

        [Authorize("Admin")]
        public IHttpResponse Delete(string ticketClass, int flightId)
        {
            var tickets = this.db.Tickets.Where(x => x.FlighId == flightId
                                                && x.Class == ticketClass
                                                && x.Customer == null).ToArray();

            this.db.Tickets.RemoveRange(tickets);
            this.db.SaveChanges();

            return this.Redirect("/Flights/Details?id=" + flightId);
        }
    }
}