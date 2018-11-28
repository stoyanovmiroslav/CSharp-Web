using Eventures.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventures.Services.Contracts
{
    public interface IOrderService
    {
        void CreateOrder(string id, int tickets, string username);

        IList<Order> GetMyOrders(string username);

        IList<Order> GetAllOrders();
    }
}
