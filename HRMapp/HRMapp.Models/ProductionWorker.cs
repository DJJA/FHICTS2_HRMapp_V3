using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public class ProductionWorker : Employee
    {
        public TeamLeader TeamLeader { get; private set; }

        public ProductionWorker(int id, string firstName, string lastName)
            : base(id, firstName, lastName)
        {
        }

        public ProductionWorker(int id, string firstName, string lastName, string phoneNumber, string emailAddress, string street, string houseNumber, string zipCode, string city, TeamLeader teamLeader)
            : base(id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city)
        {
            TeamLeader = teamLeader;
        }
    }
}
