using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    interface IHRManagerContext : IEmployeeContext<HRManager>, IContext<HRManager>
    {
    }
}
