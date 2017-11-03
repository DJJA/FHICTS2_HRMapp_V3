using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.DAL
{
    public interface IContext <T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        int Add(T value);
        bool Delete(T value);
        bool Update(T value);
    }
}
