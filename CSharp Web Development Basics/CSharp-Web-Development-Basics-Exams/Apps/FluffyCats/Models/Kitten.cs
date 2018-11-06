namespace FluffyCats.Models
{
    public class Kitten
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public Breed Breed { get; set; }

        public string Url { get; set; }
    }
}