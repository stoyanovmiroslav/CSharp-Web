using System;
using System.Collections.Generic;
using System.Text;

namespace Torshia.Models
{
    public class TaskSector
    {
        public int TaskId { get; set; }
        public Task Task { get; set; }

        public int SectorId { get; set; }
        public Sector Sector { get; set; }
    }
}