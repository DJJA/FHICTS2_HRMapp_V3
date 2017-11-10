using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    class MssqlProductionWorkerContext : IProductionWorkerContext
    {
        public IEnumerable<ProductionWorker> GetAll()
        {
            throw new NotImplementedException();
        }

        public ProductionWorker GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Add(ProductionWorker value)
        {
            throw new NotImplementedException();
        }

        public bool Delete(ProductionWorker value)
        {
            throw new NotImplementedException();
        }

        public bool Update(ProductionWorker value)
        {
            throw new NotImplementedException();
        }

        private ProductionWorker GetProductionWorkerFromDataRow(DataRow row)
        {
            var id = Convert.ToInt32(row["Id"]);
            var firstName = row["FirstName"].ToString();
            var lastName = row["LastName"].ToString();
            var phoneNumber = Convert.ToInt32(row["PhoneNumber"]);
            var emailAddress = row["EmailAddress"].ToString();
            var street = row["Street"].ToString();
            var houseNumber = row["HouseNumber"].ToString();
            var zipCode = row["ZipCode"].ToString();
            var city = row["City"].ToString();

            var Skillsets = new List<Skillset>();
            TeamLeader TeamLeader = null;
            return new ProductionWorker(id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city, Skillsets, TeamLeader);
        }

        private IEnumerable<SqlParameter> GetSqlParametersFromProductionWorker(ProductionWorker productionWorker, bool withId)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@FirstName", productionWorker.FirstName),
                new SqlParameter("@LastName", productionWorker.LastName),
                new SqlParameter("@PhoneNumber", productionWorker.PhoneNumber),
                new SqlParameter("@EmailAddress", productionWorker.EmailAddress),
                new SqlParameter("@Street", productionWorker.Street),
                new SqlParameter("@HouseNumber", productionWorker.HouseNumber),
                new SqlParameter("@ZipCode", productionWorker.ZipCode),
                new SqlParameter("@City", productionWorker.City)
            };
            if (withId)
            {
                parameters.Add(new SqlParameter("@Id", productionWorker.Id));
            }
            return parameters;
        }
    }
}
