using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Logic;
using HRMapp.Models;

namespace HRMapp.Factory
{
    public class EmployeeFactory : IFactory<IEmployeeLogic>
    {
        public IEmployeeLogic Manage()
        {
            return new EmployeeLogic(new EmployeeRepo(ContextType.Mssql));
        }
    }
}
