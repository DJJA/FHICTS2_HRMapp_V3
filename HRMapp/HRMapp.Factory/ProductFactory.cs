using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Logic;

namespace HRMapp.Factory
{
    public static class ProductFactory
    {
        public static ProductLogic ManageProducts()
        {
            return new ProductLogic(new ProductRepo(ContextType.Mssql));
        }
    }
}
