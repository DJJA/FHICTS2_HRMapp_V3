﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.DAL.Repositories
{
    public interface IRepo <T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        int Add(T value);
        void Update(T value);
    }
}
