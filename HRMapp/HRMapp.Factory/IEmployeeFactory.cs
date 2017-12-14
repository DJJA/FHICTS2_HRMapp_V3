using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Logic;
using HRMapp.Models;

namespace HRMapp.Factory
{
    public interface IEmployeeFactory : IFactory<IEmployeeLogic>
    {
    }
}
