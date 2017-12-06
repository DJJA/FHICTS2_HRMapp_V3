using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public class ProductionEmployee : Employee
    {
        public ProductionEmployee(int id) 
            : base(id)
        {
        }

        public ProductionEmployee(int id, string firstName, string lastName) 
            : base(id, firstName, lastName)
        {
        }

        protected ProductionEmployee(int id, string firstName, string lastName, string phoneNumber, string emailAddress, string street, string houseNumber, string zipCode, string city) 
            : base(id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city)
        {
        }
    }
}
