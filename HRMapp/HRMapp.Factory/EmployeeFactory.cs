using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Logic;

namespace HRMapp.Factory
{
    public class EmployeeFactory
    {
        public static EmployeeLogic ManageEmployees()
        {
            return new EmployeeLogic(new EmployeeRepo(ContextType.Mssql));
        }
    }
}
