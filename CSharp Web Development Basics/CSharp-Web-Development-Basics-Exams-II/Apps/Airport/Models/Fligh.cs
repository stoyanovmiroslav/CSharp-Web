using System;
using System.Collections.Generic;

namespace Airport.Models
{
    public class Fligh
    {
        public int Id { get; set; }

        public string Destination { get; set; }

        public string Origin { get; set; }

        public DateTime Date { get; set; }

        public DateTime Time { get; set; } //For Delete

        public string Image { get; set; }

        public bool PulicFlag { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}