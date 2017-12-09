using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.DAL
{
    internal interface IContext <T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        int Add(T value);
        void Update(T value);
    }
}
