using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Logic;
using HRMapp.Models;

namespace HRMapp.Factory
{
    public class OrderFactory : IFactory<IOrderLogic>
    {
        public IOrderLogic Manage()
        {
            return new OrderLogic(new OrderRepo(ContextType.Mssql));
        }
    }
}
