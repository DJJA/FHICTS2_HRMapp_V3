using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public class ProductionTask
    {
        private string name, description;
        public int Id { get; private set; }
        public Product Product { get; private set; }
        public string Name
        {
            get => name;
            private set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    name = value;
                }
                else
                {
                    throw new ArgumentException("De taak moet een naam hebben.");
                }
            }
        }
        public string Description
        {
            get => description;
            private set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    description = value;
                }
                else
                {
                    throw new ArgumentException("De taak moet een omschrijving hebben.");
                }
            }
        }
        public TimeSpan Duration { get; private set; }
        public List<Employee> Employees { get; private set; }
        

        public ProductionTask(int id, Product product, string name, string description, TimeSpan duration, List<Employee> employees)
        {
            Id = id;
            Product = product;
            Name = name;
            Description = description;
            Duration = duration;
            Employees = employees;
        }

        public ProductionTask(int id)
        {
            Id = id;
        }
    }
}
