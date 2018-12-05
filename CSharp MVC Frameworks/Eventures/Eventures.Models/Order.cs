using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Eventures.Models
{
    public class Order
    {
        public string Id { get; set; }

        public DateTime OrderedOn { get; set; }

        [ForeignKey("Event")]
        public string EventId { get; set; }
        public virtual Event Event { get; set; }

        public string CustomerId { get; set; }
        public virtual EventuresUser Customer { get; set; }

        public int TicketsCount { get; set; }
    }
}