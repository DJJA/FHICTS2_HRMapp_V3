using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public class TeamLeader : ProductionEmployee
    {
        public List<ProductionWorker> TeamMembers { get; private set; }

        public TeamLeader(int id, string firstName, string lastName) 
            : base(id, firstName, lastName)
        {
            TeamMembers = new List<ProductionWorker>();
        }

        public TeamLeader(int id, string firstName, string lastName, string phoneNumber, string emailAddress, string street, string houseNumber, string zipCode, string city)
            : base(id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city)
        {
            TeamMembers = new List<ProductionWorker>();
        }

        public TeamLeader(int id, string firstName, string lastName, string phoneNumber, string emailAddress, string street, string houseNumber, string zipCode, string city, List<ProductionWorker> teamMembers) 
            : base(id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city)
        {
            TeamMembers = teamMembers;
        }
    }
}
