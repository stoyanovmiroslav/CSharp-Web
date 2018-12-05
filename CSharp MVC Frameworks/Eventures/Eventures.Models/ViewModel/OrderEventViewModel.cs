using System.ComponentModel.DataAnnotations;

namespace Eventures.Models.ViewModel
{
    public class OrderEventViewModel
    {
        public string Id { get; set; }

        public string EventName { get; set; }

        public string EventPlace { get; set; }

        public string EventStart { get; set; }

        public string EventEnd { get; set; }
        
        public int TicketsCount { get; set; }
    }
}