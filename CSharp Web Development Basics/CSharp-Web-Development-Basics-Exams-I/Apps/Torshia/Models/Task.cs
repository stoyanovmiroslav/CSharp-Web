using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Torshia.Models
{
    public class Task
    {
        public Task()
        {
            this.AffectedSectors = new List<TaskSector>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsReported { get; set; }

        public string Description { get; set; }

        public string Participants { get; set; }

        public List<TaskSector> AffectedSectors { get; set; }

        [NotMapped]
        public int? ReportId { get; set; }

        public Report Report { get; set; }
    }
}
