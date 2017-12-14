using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Logic;
using HRMapp.Models;

namespace HRMapp.Factory
{
    public class TaskFactory : IFactory<ITaskLogic>
    {
        public ITaskLogic Manage()
        {
            return new TaskLogic(new TaskRepo(ContextType.Mssql));
        }
    }
}
