using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Torshia.Models.Enums;

namespace Torshia.Models
{
    public class Report
    {
        public int Id { get; set; }

        public Status Status { get; set; }

        public DateTime ReportedOn { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }

        public int ReporterId { get; set; }
        public User Reporter { get; set; }
    }
}