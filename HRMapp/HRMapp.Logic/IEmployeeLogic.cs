using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;

namespace HRMapp.Logic
{
    public interface IEmployeeLogic : ILogic<Employee>
    {

        List<TeamLeader> GetAllTeamLeaders();
        TeamLeader GetTeamLeaderById(int id);
        List<ProductionEmployee> GetAllTeamLeadersAndProductionWorkers();
    }
}
