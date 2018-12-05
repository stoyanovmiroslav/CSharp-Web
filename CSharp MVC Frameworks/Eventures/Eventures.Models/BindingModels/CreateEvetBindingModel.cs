using System;
using System.ComponentModel.DataAnnotations;

namespace Eventures.Models.BindingModels
{
    public class CreateEvetBindingModel
    {
        [Required]
        [MinLength(10)]
        public string Name { get; set; }

        [Required]
        public string Place { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int TotalTickets { get; set; }

        [Required]
        public decimal PricePerTicket { get; set; }
    }
}