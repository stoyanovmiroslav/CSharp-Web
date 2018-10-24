using MishMash.Models.Enums;
using System.Collections.Generic;

namespace MishMash.Models
{
    public class Channel
    {
        public Channel()
        {
            this.Tags = new HashSet<ChanelTag>();
            this.Followers = new HashSet<UserChanel>();
        }

        public int ChannelId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Type Type { get; set; }

        public ICollection<ChanelTag> Tags { get; set; }

        public ICollection<UserChanel> Followers { get; set; }
    }
}