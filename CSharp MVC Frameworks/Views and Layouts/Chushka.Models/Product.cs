using Chushka.Models.Enums;
using System.Collections.Generic;

namespace Chushka.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public ProductType ProductType { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}