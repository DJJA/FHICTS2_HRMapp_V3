using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Models;

namespace HRMapp.Logic
{
    public class EmployeeLogic : IEmployeeLogic
    {
        private IEmployeeRepo repo;

        public EmployeeLogic(IEmployeeRepo repo)
        {
            this.repo = repo;
        }

        public List<Employee> GetAll() => repo.GetAll().ToList();
        public Employee GetById(int id) => repo.GetById(id);
        public int Add(Employee employee) => repo.Add(employee);
        public void Update(Employee employee) => repo.Update(employee);

        public List<TeamLeader> GetAllTeamLeaders() => repo.GetAllTeamLeaders().ToList();
        public TeamLeader GetTeamLeaderById(int id) => repo.GetTeamLeaderById(id);
        public List<ProductionEmployee> GetAllTeamLeadersAndProductionWorkers() => repo.GetAllTeamLeadersAndProductionWorkers().ToList();
    }
}
