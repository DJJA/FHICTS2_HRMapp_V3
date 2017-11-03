using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMapp
{
    public class CrossActionMessageHolder
    {
        private string message;
        public string Message
        {
            get
            {
                var tempMessage = message;
                message = string.Empty;
                return tempMessage;
            }
            set
            {
                message = value;
            }
        }
    }
}
