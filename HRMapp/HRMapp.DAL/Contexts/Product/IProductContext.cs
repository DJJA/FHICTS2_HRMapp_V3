using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    interface IProductContext : IContext<Product>
    {
        IEnumerable<ProductionTask> GetRequiredTasks(int productId);
    }
}
