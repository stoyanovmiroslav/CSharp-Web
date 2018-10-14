using System;
using System.Collections.Generic;
using System.Text;

namespace CakeApp.Data.Models
{
    public class Product
    {
        public Product()
        {
            this.Orders = new HashSet<OrderProduct>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<OrderProduct> Orders { get; set; }
    }
}
