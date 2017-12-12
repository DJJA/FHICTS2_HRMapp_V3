using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Logic;

namespace HRMapp.Factory
{
    public static class OrderFactory
    {
        public static OrderLogic ManageOrders()
        {
            return new OrderLogic(new OrderRepo(ContextType.Mssql));
        }
    }
}
