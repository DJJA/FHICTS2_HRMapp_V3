using HRMapp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.DAL
{
    internal interface ITaskContext : IContext<ProductionTask>
    {
        IEnumerable<ProductionTask> GetByProductId(int productId);
        //IEnumerable<Employee> GetEmployeesByTaskId(int taskId);
    }
}
