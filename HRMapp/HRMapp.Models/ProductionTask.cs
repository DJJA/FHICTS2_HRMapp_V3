using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public class ProductionTask
    {
        private string name, description;
        public int Id { get; private set; }
        public string Name
        {
            get { return name; }
            private set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    name = value;
                }
                else
                {
                    throw new ArgumentException("Task name must be set.");
                }
            }
        }
        public string Description
        {
            get { return description; }
            private set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    description = value;
                }
                else
                {
                    throw new ArgumentException("Task description must be set.");
                }
            }
        }
        public TimeSpan Duration { get; private set; }
        public List<Skillset> RequiredSkillsets { get; private set; }

        public ProductionTask(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            RequiredSkillsets = new List<Skillset>();
        }

        public ProductionTask(int id, string name, string description, TimeSpan duration, List<Skillset> requiredSkillsets)
            : this(id, name, description)
        {
            Duration = duration;
            RequiredSkillsets = requiredSkillsets;
        }
    }
}
