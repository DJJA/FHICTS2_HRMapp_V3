using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models.Exceptions
{
    public class DBSkillsetException : DBException
    {
        public DBSkillsetException(string message)
            :base(message)
        {
            
        }
    }
}
