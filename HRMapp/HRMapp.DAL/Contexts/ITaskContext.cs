using HRMapp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.DAL
{
    public interface ITaskContext : IContext<ProductionTask>
    {
        IEnumerable<Skillset> GetRequiredSkillsets(int taskId);
        bool UpdateRequiredSkillsets(ProductionTask task);
    }
}
