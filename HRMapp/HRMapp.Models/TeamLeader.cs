using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public class TeamLeader : Employee
    {
        public List<Skillset> Skillsets { get; private set; }
        public List<ProductionWorker> TeamMembers { get; private set; }

        public TeamLeader(int id, string firstName, string lastName) 
            : base(id, firstName, lastName)
        {
            Skillsets = new List<Skillset>();
            TeamMembers = new List<ProductionWorker>();
        }

        public TeamLeader(int id, string firstName, string lastName, int phoneNumber, string emailAddress, string street, string houseNumber, string zipCode, string city, List<Skillset> skillsets, List<ProductionWorker> teamMembers) 
            : base(id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city)
        {
            Skillsets = skillsets;
            TeamMembers = teamMembers;
        }
    }
}
