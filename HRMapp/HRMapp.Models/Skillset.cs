using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public class Skillset
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
                    throw new ArgumentException("Skillset name must be set.");
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
                    throw new ArgumentException("Skillset description must be set.");
                }
            }
        }

        public Skillset(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public Skillset(int id, string name, string description)
            : this(name, description)
        {
            Id = id;
        }
    }
}
