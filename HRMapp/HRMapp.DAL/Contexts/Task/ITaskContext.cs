using HRMapp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.DAL.Contexts
{
    internal interface ITaskContext : IContext<ProductionTask>
    {
        IEnumerable<ProductionTask> GetByProductId(int productId);
        void Delete(ProductionTask task);
    }
}
