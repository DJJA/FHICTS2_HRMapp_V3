using HRMapp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Contexts;

namespace HRMapp.DAL.Repositories
{
    public class TaskRepo : IRepo
    {
        private ITaskContext context = new MssqlTaskContext();

        public IEnumerable<ProductionTask> GetAll() => context.GetAll();
        public ProductionTask GetById(int id) => context.GetById(id);
        public int Add(ProductionTask task) => context.Add(task);
        public bool Update(ProductionTask task) => context.Update(task);
        public void Delete(ProductionTask task) => context.Delete(task);
        public IEnumerable<ProductionTask> GetByProductId(int id) => context.GetByProductId(id);
    }
}
