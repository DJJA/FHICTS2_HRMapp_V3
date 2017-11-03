using HRMapp.Models;
using HRMapp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Logic
{
    public class SkillsetLogic
    {
        private SkillsetRepo repo = new SkillsetRepo();

        public IEnumerable<Skillset> GetAll() => repo.GetAll();
        public Skillset GetById(int id) => repo.GetById(id);
        public int Add(Skillset skillset) => repo.Add(skillset);
        public bool Update(Skillset skillset) => repo.Update(skillset);
    }
}
