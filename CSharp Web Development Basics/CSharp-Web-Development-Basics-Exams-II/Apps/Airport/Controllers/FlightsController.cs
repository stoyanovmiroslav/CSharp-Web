using Airport.Flights;
using Airport.Models;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Globalization;
using System.Linq;

namespace Airport.Controllers
{
    public class FlightsController : BaseController
    {
        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Search(AddEditFlightsViewModel model)
        {
            var flights = this.db.Flighs.Where(x => x.Destination == model.Destination 
                                              && x.Origin == model.Origin
                                              && x.PulicFlag == true
                                              && x.Date.Day == model.Date.Day
                                              && x.Date.Month == model.Date.Month
                                              && x.Date.Year == model.Date.Year).ToArray();


            return this.View("/Home/Flights", flights);
        }

        [Authorize("Admin")]
        public IHttpResponse Edit(int id)
        {
            var flight = this.db.Flighs.FirstOrDefault(x => x.Id == id);

            var editViewModel = new EditFlightViewModel
            {
                Id = flight.Id,
                Destination = flight.Destination,
                Origin = flight.Origin,
                Date = flight.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                Time = flight.Date.ToString("HH:mm", CultureInfo.InvariantCulture),
                Image = flight.Image
             };

            return this.View(editViewModel);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Edit(AddEditFlightsViewModel model)
        {
            var flight = this.db.Flighs.FirstOrDefault(x => x.Id == model.Id);

            var flightDateTime = model.Date.AddHours(model.Time.Hour)
                                       .AddMinutes(model.Time.Minute);

            flight.Origin = model.Origin;
            flight.Destination = model.Destination;
            flight.Origin = model.Origin;
            flight.Date = flightDateTime;
            flight.Image = model.Image;

            this.db.SaveChanges();

            return this.Redirect("/Flights/Details?id=" + model.Id);
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {

            Fligh flight = null;

            if (this.User.Role == "Admin")
            {
                flight = this.db.Flighs.Include(x => x.Tickets)
                                       .FirstOrDefault(x => x.Id == id);
            }
            else
            {
                flight = this.db.Flighs.Include(x => x.Tickets)
                                  .Where(x => x.PulicFlag == true)
                                  .FirstOrDefault(x => x.Id == id);
            }

            var tickets = this.db.Tickets.Where(x => x.FlighId == id && x.Customer == null).ToArray();

            if (flight == null)
            {
                return this.BadRequestError("Invalid flight");
            }


            var ticketsByType = tickets.GroupBy(x => new
                                                            {
                                                                Class = x.Class,
                                                                Price = x.Price
                                                            })
                                                            .Select(x => new TicketsByTypeViewModel
                                                            {
                                                                Class = x.Key.Class,
                                                                Price = x.Key.Price
                                                            }).ToList();

            DetailsFlightViewModel flightViewModel = new DetailsFlightViewModel
            {
                Id = flight.Id,
                Date = flight.Date,
                Destination = flight.Destination,
                Origin = flight.Origin,
                Image = flight.Image,
                Tickets = ticketsByType,
                PulicFlag = flight.PulicFlag
            };

            return this.View(flightViewModel);
        }

        [Authorize("Admin")]
        public IHttpResponse Publish(int id)
        {
            var flight = this.db.Flighs.FirstOrDefault(x => x.Id == id);
            flight.PulicFlag = true;
            this.db.SaveChanges();

            return this.Redirect("/Flights/Details?id=" + id);
        }

        [Authorize("Admin")]
        public IHttpResponse Add()
        {
            return this.View();
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Add(AddEditFlightsViewModel model)
        {

            var flightDate = model.Date.AddHours(model.Time.Hour)
                                       .AddMinutes(model.Time.Minute);


            var flight = new Fligh
            {
                Image = model.Image,
                Date = flightDate,
                Destination = model.Destination,
                Origin = model.Origin,
                PulicFlag = false
            };

            this.db.Flighs.Add(flight);
            this.db.SaveChanges();

            return this.Redirect("/Home/Flights");
        }
    }

    public class TicketsByTypeViewModel
    {

        public string Class { get; set; }

        public decimal Price { get; set; }
    }

    public class AddEditFlightsViewModel
    {
        public int Id { get; set; }

        public string Destination { get; set; }

        public string Origin { get; set; }

        public DateTime Date { get; set; }

        public DateTime Time { get; set; }

        public string Image { get; set; }
    }
}