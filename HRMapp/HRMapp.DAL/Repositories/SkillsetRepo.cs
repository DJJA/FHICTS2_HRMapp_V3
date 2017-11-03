using HRMapp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Contexts;

namespace HRMapp.DAL.Repositories
{
    public class SkillsetRepo : IRepo
    {
        private ISkillsetContext context = new MssqlSkillsetContext();

        public IEnumerable<Skillset> GetAll() => context.GetAll();
        public Skillset GetById(int id) => context.GetById(id);
        public int Add(Skillset skillset) => context.Add(skillset);
        public bool Update(Skillset skillset) => context.Update(skillset);
    }
}
