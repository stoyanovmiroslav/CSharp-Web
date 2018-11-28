using Eventures.Data;
using Eventures.Models;
using Eventures.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eventures.Services
{
    public class OrderService : IOrderService
    {
        private EventuresDbContext db;

        public OrderService(EventuresDbContext db)
        {
            this.db = db;
        }

        public void CreateOrder(string eventId, int ticketsCount, string username)
        {
            var user = this.db.Users.Include(x => x.Orders).FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return;
            }

            var eventa = this.db.Events.FirstOrDefault(x => x.Id == eventId);

            var order = new Order
            {
                Event = eventa,
                CustomerId = user.Id,
                OrderedOn = DateTime.UtcNow,
                TicketsCount = ticketsCount,
            };

            user.Orders.Add(order);
            this.db.SaveChanges();
        }

        public IList<Order> GetAllOrders()
        {
            return this.db.Orders.Include(x => x.Event)
                                 .Include(x => x.Customer).ToList();
        }

        public IList<Order> GetMyOrders(string username)
        {
            return this.db.Orders.Include(x => x.Event)
                                 .Where(x => x.Customer.UserName == username).ToList();
        }
    }
}
