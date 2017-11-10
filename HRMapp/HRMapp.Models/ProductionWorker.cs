using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public class ProductionWorker : Employee
    {
        public List<Skillset> Skillsets { get; private set; }
        public TeamLeader TeamLeader { get; private set; }

        public ProductionWorker(int id, string firstName, string lastName)
            : base(id, firstName, lastName)
        {
            Skillsets = new List<Skillset>();
        }

        public ProductionWorker(int id, string firstName, string lastName, int phoneNumber, string emailAddress, string street, string houseNumber, string zipCode, string city, List<Skillset> skillsets, TeamLeader teamLeader)
            : base(id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city)
        {
            Skillsets = skillsets;
            TeamLeader = teamLeader;
        }
    }
}
