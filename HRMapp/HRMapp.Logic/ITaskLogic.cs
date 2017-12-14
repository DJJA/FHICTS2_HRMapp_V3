using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;

namespace HRMapp.Logic
{
    public interface ITaskLogic : ILogic<ProductionTask>
    {

        void Delete(ProductionTask task);
        List<ProductionTask> GetByProductId(int id);
    }
}
