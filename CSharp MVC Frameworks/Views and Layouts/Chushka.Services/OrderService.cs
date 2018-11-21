using Chushka.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Chushka.Data;
using Chushka.Services.Contracts;

namespace Chushka.Services
{
    public class OrderService : IOrderService
    {
        private ChushkaDbContext db;

        public OrderService(ChushkaDbContext db)
        {
            this.db = db;
        }

        public bool CreateOrder(Product product, string userName)
        {
            var user = this.db.Users.FirstOrDefault(x => x.UserName == userName);

            if (user == null)
            {
                return false;
            }

            var order = new Order
            {
                Product = product,
                Client = user,
                OrderedOn = DateTime.UtcNow
            };

            this.db.Orders.Add(order);
            this.db.SaveChanges();

            return true;
        }

        public Order[] GetAllOrders()
        {
            var orders = this.db.Orders.Include(x => x.Client)
                                       .Include(x => x.Product)
                                       .ToArray();
            return orders;
        }
    }
}