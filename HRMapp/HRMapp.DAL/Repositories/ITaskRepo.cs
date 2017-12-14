using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Repositories
{
    public interface ITaskRepo : IRepo<ProductionTask>
    {
        void Delete(ProductionTask task);
        IEnumerable<ProductionTask> GetByProductId(int productId);
    }
}
