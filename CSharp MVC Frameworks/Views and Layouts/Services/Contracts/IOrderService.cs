using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Services.Contracts
{
    public interface IOrderService
    {
        bool CreateOrder(Product product, string user);

        Order[] GetAllOrders();
    }
}