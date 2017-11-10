using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    class MssqlHRManagerContext : IHRManager
    {
        public IEnumerable<HRManager> GetAll()
        {
            throw new NotImplementedException();
        }

        public HRManager GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Add(HRManager value)
        {
            throw new NotImplementedException();
        }

        public bool Delete(HRManager value)
        {
            throw new NotImplementedException();
        }

        public bool Update(HRManager value)
        {
            throw new NotImplementedException();
        }
    }
}
