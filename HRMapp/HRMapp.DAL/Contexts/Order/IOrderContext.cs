using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    interface IOrderContext : IContext<Order>
    {
        List<OrderItem> GetOrderItems(int orderId);
    }
}
