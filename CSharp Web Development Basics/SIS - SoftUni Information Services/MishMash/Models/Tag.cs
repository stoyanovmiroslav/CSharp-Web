using System.Collections.Generic;

namespace MishMash.Models
{
    public class Tag
    {
        public Tag()
        {
            this.Chanels = new HashSet<ChanelTag>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<ChanelTag> Chanels { get; set; }
    }
}