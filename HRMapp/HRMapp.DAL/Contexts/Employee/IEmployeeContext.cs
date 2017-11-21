using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts   // TODO fix namespaces
{
    interface IEmployeeContext<T>
    {
        bool ChangeToThisTypeAndUpdate(T employee);
    }
}
