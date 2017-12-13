using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Logic;

namespace HRMapp.Factory
{
    public static class TaskFactory
    {
        public static TaskLogic ManageTasks()
        {
            return new TaskLogic(new TaskRepo(ContextType.Mssql));
        }
    }
}
