using System;
using System.Collections.Generic;
using Torshia.Models;

namespace Torshia.ViewModels
{
    public class DetailsTaskViewModel
    {
        public string Title { get; set; }

        public string DueDate { get; set; }

        public bool IsReported { get; set; }

        public string Description { get; set; }

        public string Participants { get; set; }

        public string AffectedSectors { get; set; }

        public int AffectedSectorsCount { get; set; }
    }
}