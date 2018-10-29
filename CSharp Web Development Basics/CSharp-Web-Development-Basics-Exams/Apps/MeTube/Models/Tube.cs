namespace MeTube.Models
{
    public class Tube
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string YoutubeId { get; set; }

        public int Views { get; set; } = 0;

        public int UserId { get; set; }
        public User User { get; set; }
    }
}