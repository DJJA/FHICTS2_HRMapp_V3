using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Logic
{
    public interface ILogic <T>
    {
        List<T> GetAll();
        T GetById(int id);
        int Add(T value);
        void Update(T value);
    }
}
