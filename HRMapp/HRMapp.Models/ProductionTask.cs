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
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("De taak moet een naam hebben.");
                }
                name = value;
            }
        }
        public string Description
        {
            get => description;
            private set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("De taak moet een omschrijving hebben.");
                }
                description = value;
            }
        }
        public TimeSpan Duration { get; private set; }
        public List<ProductionEmployee> Employees { get; private set; }
        

        public ProductionTask(int id, Product product, string name, string description, TimeSpan duration, List<ProductionEmployee> employees)
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
