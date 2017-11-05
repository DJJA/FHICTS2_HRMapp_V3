using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models.Exceptions
{
    public class DBException : Exception
    {
        public DBException(string message)
            :base(message)
        {
            
        }
    }
}
