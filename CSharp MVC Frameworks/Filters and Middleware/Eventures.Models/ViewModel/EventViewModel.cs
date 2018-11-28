using System.ComponentModel.DataAnnotations;

namespace Eventures.Models.ViewModel
{
    public class EventViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Place { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        [Display(Name = "Tickets")]
        [Range(0, 20)]
        public int TicketsCount { get; set; }
    }
}