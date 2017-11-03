using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;
using System.Linq;

namespace HRMapp.DAL
{
    public class MemoryTaskContext : ITaskContext
    {
        private static List<ProductionTask> tasks = new List<ProductionTask>();

        public MemoryTaskContext()
        {
            if (tasks.Count() == 0) AddRandomItems();
        }

        public int Add(ProductionTask value)
        {
            var newTask = new ProductionTask(tasks.Count, value.Name, value.Description);
            tasks.Add(newTask);
            return newTask.Id;
        }

        public bool Delete(ProductionTask value)
        {
            tasks.Remove(value);
            return true;
        }

        public IEnumerable<ProductionTask> GetAll()
        {
            return tasks;
        }

        public ProductionTask GetById(int id)
        {
            return tasks.Single(task => task.Id == id);
        }

        public bool Update(ProductionTask value)
        {
            //var item = list.Single(task => task.Id == value.Id);
            //item = value;
            try
            {
                tasks[tasks.FindIndex(task => task.Id == value.Id)] = value;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Skillset> GetRequiredSkillsets(int taskId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRequiredSkillsets(ProductionTask task)
        {
            throw new NotImplementedException();
        }

        private void AddRandomItems()
        {
            //var mem = new MemorySkillsetContext();
            //var skillsets = mem.GetAll().ToList();
            //tasks.Add(new ProductionTask(0, "Connector solderen 1", "Het solderen van een 4-polige connector", new TimeSpan(2, 30, 0), new List<Skillset>() { skillsets[0], skillsets[1] }));
            //tasks.Add(new ProductionTask(1, "Connector solderen 2", "Het solderen van een 8-polige connector", new TimeSpan(5, 0, 0), new List<Skillset>() { skillsets[3], skillsets[2] }));
            //tasks.Add(new ProductionTask(2, "Connector solderen 3", "Het solderen van een 16-polige connector", new TimeSpan(4, 30, 0), new List<Skillset>() { skillsets[4], skillsets[1] }));
            //tasks.Add(new ProductionTask(3, "Connector solderen 4", "Het solderen van een 32-polige connector", new TimeSpan(1, 30, 0), new List<Skillset>() { skillsets[0], skillsets[2] }));
            //tasks.Add(new ProductionTask(4, "Connector solderen 5", "Het solderen van een 64-polige connector", new TimeSpan(0, 45, 0), new List<Skillset>() { skillsets[3], skillsets[4] }));
            

            tasks.Add(new ProductionTask(0, "Connector solderen 1", "Het solderen van een 4-polige connector"));
            tasks.Add(new ProductionTask(1, "Connector solderen 2", "Het solderen van een 8-polige connector"));
            tasks.Add(new ProductionTask(2, "Connector solderen 3", "Het solderen van een 16-polige connector"));
            tasks.Add(new ProductionTask(3, "Connector solderen 4", "Het solderen van een 32-polige connector"));
            tasks.Add(new ProductionTask(4, "Connector solderen 5", "Het solderen van een 64-polige connector"));
        }
    }
}
