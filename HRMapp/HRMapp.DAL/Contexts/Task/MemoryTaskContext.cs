using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;
using System.Linq;

namespace HRMapp.DAL.Contexts
{
    internal class MemoryTaskContext : ITaskContext
    {
        private static List<ProductionTask> tasks = new List<ProductionTask>();

        public MemoryTaskContext()
        {
            if (tasks.Count() == 0) AddRandomItems();
        }

        public int Add(ProductionTask value)
        {
            var newTask = new ProductionTask(tasks.Count, new Product(1),  value.Name, value.Description, value.Duration, value.Employees);
            tasks.Add(newTask);
            return newTask.Id;
        }

        public void Delete(ProductionTask value)
        {
            tasks.Remove(value);
        }

        public IEnumerable<ProductionTask> GetAll()
        {
            return tasks;
        }

        public ProductionTask GetById(int id)
        {
            return tasks.Single(task => task.Id == id);
        }

        public void Update(ProductionTask value)
        {
            try
            {
                tasks[tasks.FindIndex(task => task.Id == value.Id)] = value;
            }
            catch
            {
            }
        }

        public IEnumerable<ProductionTask> GetByProductId(int productId)
        {
            throw new NotImplementedException();
        }

        private void AddRandomItems()
        {
            tasks.Add(new ProductionTask(0, new Product(1),  "Connector solderen 1", "Het solderen van een 4-polige connector", new TimeSpan(), new List<ProductionEmployee>()));
            tasks.Add(new ProductionTask(1, new Product(1), "Connector solderen 2", "Het solderen van een 8-polige connector", new TimeSpan(), new List<ProductionEmployee>()));
            tasks.Add(new ProductionTask(2, new Product(1), "Connector solderen 3", "Het solderen van een 16-polige connector", new TimeSpan(), new List<ProductionEmployee>()));
            tasks.Add(new ProductionTask(3, new Product(1), "Connector solderen 4", "Het solderen van een 32-polige connector", new TimeSpan(), new List<ProductionEmployee>()));
            tasks.Add(new ProductionTask(4, new Product(1), "Connector solderen 5", "Het solderen van een 64-polige connector", new TimeSpan(), new List<ProductionEmployee>()));
        }
    }
}
