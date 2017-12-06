using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Models;

namespace HRMapp.Logic
{
    public class EmployeeLogic
    {
        private EmployeeRepo repo = new EmployeeRepo();

        public IEnumerable<Employee> GetAll() => repo.GetAll();
        public Employee GetById(int id) => repo.GetById(id);
        public int Add(Employee employee) => repo.Add(employee);
        public bool Update(Employee employee) => repo.Update(employee);

        public IEnumerable<TeamLeader> GetAllTeamLeaders => repo.GetAllTeamLeaders();
        public TeamLeader GetTeamLeaderById(int id) => repo.GetTeamLeaderById(id);
        public List<ProductionEmployee> GetAllTeamLeadersAndProductionWorkers => repo.GetAllTeamLeadersAndProductionWorkers().ToList();
    }
}
