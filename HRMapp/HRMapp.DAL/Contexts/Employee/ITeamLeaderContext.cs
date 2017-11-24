using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    interface ITeamLeaderContext : IEmployeeContext<TeamLeader>, IContext<TeamLeader>
    {
        IEnumerable<Skillset> GetSkillsets(int employeeId);
        void UpdateSkillsets(TeamLeader teamLeader);
        IEnumerable<ProductionWorker> GetTeamMembers(int teamLeaderId);
    }
}
