using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    interface IProductionWorkerContext : IEmployeeContext<ProductionWorker>, IContext<ProductionWorker>
    {
    }
}
