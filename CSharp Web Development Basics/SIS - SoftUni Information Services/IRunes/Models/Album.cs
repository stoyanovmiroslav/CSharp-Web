using System.Collections.Generic;
using System.Linq;

namespace IRunes.Models
{
    public class Album
    {
        public Album()
        {
            this.Tracks = new HashSet<Track>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal? Price => this.Tracks?.Sum(x => x.Price);

        public decimal? PriceAfterDiscount => this.Price - (this.Price * 0.13M);

        public ICollection<Track> Tracks { get; set; }
    }
}
