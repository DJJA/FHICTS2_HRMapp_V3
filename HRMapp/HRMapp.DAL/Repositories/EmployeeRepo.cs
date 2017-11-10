using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Contexts;
using HRMapp.Models;

namespace HRMapp.DAL.Repositories
{
    public class EmployeeRepo : IRepo
    {
        private IProductionWorkerContext productionWorkerContext = new MssqlProductionWorkerContext();
        private ITeamLeaderContext teamLeaderContext = new MssqlTeamLeaderContext();
        private IHRManager hrManagerContext = new MssqlHRManagerContext();
        private ISalesManagerContext salesManagerContext = new MssqlSalesManagerContext();

        public IEnumerable<Employee> GetAll()
        {
            var employees = new List<Employee>();
            employees.AddRange(teamLeaderContext.GetAll());
            employees.AddRange(productionWorkerContext.GetAll());
            employees.AddRange(hrManagerContext.GetAll());
            employees.AddRange(salesManagerContext.GetAll());
            return employees;
        }

        public Employee GetById(int id)
        {
            Employee employee = productionWorkerContext.GetById(id);
            if (employee != null) return employee;
            employee = teamLeaderContext.GetById(id);
            if (employee != null) return employee;
            employee = hrManagerContext.GetById(id);
            if (employee != null) return employee;
            employee = salesManagerContext.GetById(id);
            return employee;
        }

        public int Add(Employee employee)
        {
            if (employee is ProductionWorker)
                return productionWorkerContext.Add((ProductionWorker)employee);
            if (employee is TeamLeader)
                return teamLeaderContext.Add((TeamLeader)employee);
            if (employee is HRManager)
                return hrManagerContext.Add((HRManager)employee);
            if (employee is SalesManager)
                return salesManagerContext.Add((SalesManager)employee);
            throw new ArgumentException("Type of employee is not supported in the 'Add' method (DAL -> EmployeeRepo)");
        }

        public bool Update(Employee employee)
        {
            if (employee is ProductionWorker)
                return productionWorkerContext.Update((ProductionWorker)employee);
            if (employee is TeamLeader)
                return teamLeaderContext.Update((TeamLeader)employee);
            if (employee is HRManager)
                return hrManagerContext.Update((HRManager)employee);
            if (employee is SalesManager)
                return salesManagerContext.Update((SalesManager)employee);
            throw new ArgumentException("Type of employee is not supported in the 'Update' method (DAL -> EmployeeRepo)");
        }
    }
}