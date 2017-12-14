using HRMapp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Contexts;

namespace HRMapp.DAL.Repositories
{
    public class TaskRepo : ITaskRepo
    {
        private ITaskContext context;

        public TaskRepo(ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.Mssql:
                    context = new MssqlTaskContext();
                    break;
                case ContextType.Memmory:
                    context = new MemoryTaskContext();
                    break;
                default: throw new NotImplementedException();
            }
        }

        public IEnumerable<ProductionTask> GetAll() => context.GetAll();
        public ProductionTask GetById(int id) => context.GetById(id);
        public int Add(ProductionTask task) => context.Add(task);
        public void Update(ProductionTask task) => context.Update(task);
        public void Delete(ProductionTask task) => context.Delete(task);
        public IEnumerable<ProductionTask> GetByProductId(int productId) => context.GetByProductId(productId);
    }
}
