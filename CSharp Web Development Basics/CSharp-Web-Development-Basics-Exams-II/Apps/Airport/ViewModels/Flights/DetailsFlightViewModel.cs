using Airport.Controllers;
using System;
using System.Collections.Generic;

namespace Airport.Flights
{
    public class DetailsFlightViewModel
    {
        public int Id { get; set; }

        public string Destination { get; set; }

        public string Origin { get; set; }

        public DateTime Date { get; set; }

        public string Image { get; set; }

        public bool PulicFlag { get; set; }

        public virtual List<TicketsByTypeViewModel> Tickets { get; set; }
    }
}
