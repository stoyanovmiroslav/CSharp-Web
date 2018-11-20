using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        [Range(0, 9999.99)]
        public decimal Price { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(100)]
        public string Description { get; set; }

        public string ProductType { get; set; }

        public string DisableValue { get; set; }
    }
}