using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public class Product
    {
        private string name;

        public int Id { get; private set; }

        public string Name
        {
            get => name;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Naam moet ingevuld worden.");
                }
                name = value;
            }
        }

        public string Description { get; private set; }

        public List<ProductionTask> Tasks { get; private set; }

        public Product(int id)
        {
            Id = id;
        }

        public Product(int id, string name, string description, List<ProductionTask> tasks)
        {
            Id = id;
            Name = name;
            Description = description;
            Tasks = tasks;
        }
    }
}
