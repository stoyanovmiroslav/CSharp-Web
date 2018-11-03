namespace Airport.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public string Class { get; set; }

        public int? CustomerId { get; set; }
        public virtual User Customer { get; set; }

        public int FlighId { get; set; }
        public virtual Fligh Fligh { get; set; }
    }
}