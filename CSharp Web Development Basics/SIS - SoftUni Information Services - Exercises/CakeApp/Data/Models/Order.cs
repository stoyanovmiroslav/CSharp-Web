using System;
using System.Collections.Generic;
using System.Text;

namespace CakeApp.Data.Models
{
    public class Order
    {
        public Order()
        {
            this.Products = new HashSet<OrderProduct>();
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public DateTime DateOfCreation { get; set; }

        public ICollection<OrderProduct> Products { get; set; }
    }
}
