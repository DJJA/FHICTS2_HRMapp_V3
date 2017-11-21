using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    interface ISalesManagerContext : IEmployeeContext<SalesManager>, IContext<SalesManager>
    {
    }
}
