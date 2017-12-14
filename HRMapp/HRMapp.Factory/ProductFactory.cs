using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Logic;
using HRMapp.Models;

namespace HRMapp.Factory
{
    public class ProductFactory : IFactory<IProductLogic>
    {
        public IProductLogic Manage()
        {
            return new ProductLogic(new ProductRepo(ContextType.Mssql));
        }
    }
}
