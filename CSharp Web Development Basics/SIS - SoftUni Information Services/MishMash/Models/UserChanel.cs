using System;
using System.Collections.Generic;
using System.Text;

namespace MishMash.Models
{
    public class UserChanel
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}