using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;
using System.Linq;

namespace HRMapp.DAL
{
    public class MemorySkillsetContext : ISkillsetContext
    {
        private static List<Skillset> skillsets = new List<Skillset>();  // Static because the homecontroller gets reïnstatiated every request, so does the entire repository pattern

        public MemorySkillsetContext()
        {
            if (skillsets.Count() == 0) AddRandomItems();
        }

        public int Add(Skillset value)
        {
            var skillset = new Skillset(skillsets[skillsets.Count - 1].Id + 1, value.Name, value.Description);
            skillsets.Add(skillset);
            return skillset.Id;
        }

        public bool Delete(Skillset value)
        {
            skillsets.Remove(value);
            return true;
        }

        public IEnumerable<Skillset> GetAll()
        {
            return skillsets;
        }

        public Skillset GetById(int id)
        {
            return skillsets.Single(skillset => skillset.Id == id);
        }

        public bool Update(Skillset value)
        {
            try
            {
                skillsets[skillsets.FindIndex(skillset => skillset.Id == value.Id)] = value;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void AddRandomItems()
        {
            skillsets.Add(new Skillset(0, "Solderen 1", "Het kunnen solderen van een 4-polige connector."));
            skillsets.Add(new Skillset(1, "Solderen 2", "Het kunnen solderen van een 8-polige connector."));
            skillsets.Add(new Skillset(2, "Solderen 3", "Het kunnen solderen van een 16-polige connector."));
            skillsets.Add(new Skillset(3, "Solderen 4", "Het kunnen solderen van een 32-polige connector."));
            skillsets.Add(new Skillset(4, "Solderen 5", "Het kunnen solderen van een 64-polige connector."));
        }
    }
}
