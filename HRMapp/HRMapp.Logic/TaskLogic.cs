using HRMapp.Models;
using HRMapp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRMapp.DAL.Repositories;

namespace HRMapp.Logic
{
    public class TaskLogic
    {
        private TaskRepo repo = new TaskRepo();
        
        public List<ProductionTask> GetAll => repo.GetAll().ToList();
        public ProductionTask GetById(int id) => repo.GetById(id);
        public int Add(ProductionTask task) => repo.Add(task);
        public bool Update(ProductionTask task) => repo.Update(task);
    }
}
