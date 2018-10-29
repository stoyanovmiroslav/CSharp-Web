using System.Collections.Generic;
using Torshia.Models;

namespace Torshia.ViewModels.Report
{
    public class ReportDetailsViewModel
    {
        public int ReportId { get; set; }

        public string Status { get; set; }

        public string ReportedOn { get; set; }

        public string Reporter { get; set; }

        public string Title { get; set; }

        public string DueDate { get; set; }

        public bool IsReported { get; set; }

        public string Description { get; set; }

        public string Participants { get; set; }

        public string AffectedSectors { get; set; }

        public int AffectedSectorsCount { get; set; }
    }
}