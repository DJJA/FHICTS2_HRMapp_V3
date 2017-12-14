using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Repositories
{
    public interface IEmployeeRepo : IRepo<Employee>
    {
        TeamLeader GetTeamLeaderById(int id);
        IEnumerable<TeamLeader> GetAllTeamLeaders();
        IEnumerable<ProductionEmployee> GetAllTeamLeadersAndProductionWorkers();
    }
}
