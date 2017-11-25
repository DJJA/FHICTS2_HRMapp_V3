using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    interface IProductContext : IContext<Product>
    {
        IEnumerable<Product> GetRequiredTasks(int productId);
        bool UpdateRequiredTasks(Product product);
    }
}
