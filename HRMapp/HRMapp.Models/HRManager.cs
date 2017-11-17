using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public class HRManager : Employee
    {
        public HRManager(int id, string firstName, string lastName) 
            : base(id, firstName, lastName)
        {
        }

        public HRManager(int id, string firstName, string lastName, string phoneNumber, string emailAddress, string street, string houseNumber, string zipCode, string city) 
            : base(id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city)
        {
        }
    }
}
