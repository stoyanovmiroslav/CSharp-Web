﻿using System;
using System.Collections.Generic;
using System.Text;

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

        public decimal Price { get; set; }

        public ICollection<Track> Tracks { get; set; }
    }
}
