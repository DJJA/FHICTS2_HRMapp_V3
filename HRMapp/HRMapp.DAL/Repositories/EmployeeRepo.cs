using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using HRMapp.DAL.Contexts;
using HRMapp.Models;

namespace HRMapp.DAL.Repositories
{
    public class EmployeeRepo : IRepo
    {
        private IProductionWorkerContext productionWorkerContext = new MssqlProductionWorkerContext();
        private ITeamLeaderContext teamLeaderContext = new MssqlTeamLeaderContext();
        private IHRManagerContext hrManagerContext = new MssqlHRManagerContext();
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
            if (employee is ProductionWorker worker)
                return productionWorkerContext.Add(worker);
            if (employee is TeamLeader leader)
                return teamLeaderContext.Add(leader);
            if (employee is HRManager hrManager)
                return hrManagerContext.Add(hrManager);
            if (employee is SalesManager manager)
                return salesManagerContext.Add(manager);
            throw new ArgumentException("Type of employee is not supported in the 'Add' method (DAL -> EmployeeRepo)");
        }

        public void Update(Employee employee)
        {
            var employeeInDatabase = GetById(employee.Id);

            if (Employee.GetTypeOfEmployee(employeeInDatabase) == Employee.GetTypeOfEmployee(employee))
            {
                if (employee is ProductionWorker worker)
                    productionWorkerContext.Update(worker);
                else if (employee is TeamLeader leader)
                    teamLeaderContext.Update(leader);
                else if (employee is HRManager manager)
                    hrManagerContext.Update(manager);
                else if (employee is SalesManager salesManager)
                    salesManagerContext.Update(salesManager);
                else
                    throw new ArgumentException("Type of employee is not supported in the 'Update' method (DAL -> EmployeeRepo)");
            }
            else
            {
                if (employee is ProductionWorker worker)
                    productionWorkerContext.ChangeToThisTypeAndUpdate(worker);
                else if (employee is TeamLeader leader)
                    teamLeaderContext.ChangeToThisTypeAndUpdate(leader);
                else if (employee is HRManager manager)
                    hrManagerContext.ChangeToThisTypeAndUpdate(manager);
                else if (employee is SalesManager salesManager)
                    salesManagerContext.ChangeToThisTypeAndUpdate(salesManager);
                else
                    throw new ArgumentException("Type of employee is not supported in the 'Update' method (DAL -> EmployeeRepo)");
            }

        }

        public TeamLeader GetTeamLeaderById(int id) => teamLeaderContext.GetById(id);

        public IEnumerable<TeamLeader> GetAllTeamLeaders() => teamLeaderContext.GetAll();

        public IEnumerable<ProductionEmployee> GetAllTeamLeadersAndProductionWorkers()
        {
            var employees = new List<ProductionEmployee>();
            employees.AddRange(teamLeaderContext.GetAll());
            employees.AddRange(productionWorkerContext.GetAll());
            return employees;
        }
    }
}