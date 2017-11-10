using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    class MssqlEmployeeContext
    {
        //public IEnumerable<Employee> GetAll()
        //{
        //    var employees = new List<Skillset>();
        //    try
        //    {
        //        var dt = GetDataViaProcedure("sp_GetSkillsets");
        //        employees.AddRange(from DataRow row in dt.Rows select GetEmployeeFromDataRow(row));
        //    }
        //    catch (SqlException sqlEx)
        //    {
        //        throw HandleGenericSqlException(sqlEx);
        //    }

        //    return employees;
        //}

        public Employee GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Add(Employee value)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Employee value)
        {
            throw new NotImplementedException();
        }

        public bool Update(Employee value)
        {
            throw new NotImplementedException();
        }

        private Employee GetEmployeeFromDataRow(DataRow row, Employee employeeType)
        {
            var Id = Convert.ToInt32(row["Id"]);
            var firstName = row["FirstName"].ToString();
            var lastName = row["LastName"].ToString();
            var phoneNumber = Convert.ToInt32(row["PhoneNumber"]);
            var emailAddress = row["EmailAddress"].ToString();
            var street = row["Street"].ToString();
            var houseNumber = row["HouseNumber"].ToString();
            var zipCode = row["ZipCode"].ToString();
            var city = row["City"].ToString();

            if (employeeType is ProductionWorker)
            {
                var Skillsets = new List<Skillset>();
                TeamLeader TeamLeader = null;
                return new ProductionWorker(Id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city, Skillsets, TeamLeader);
            }
            if (employeeType is TeamLeader)
            {
                var skillsets = new List<Skillset>();
                var TeamMembers = new List<ProductionWorker>();
                return new TeamLeader(Id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city, skillsets, TeamMembers);
            }
            if (employeeType is HRManager)
            {
                return new HRManager(Id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city);
            }
            if (employeeType is SalesManager)
            {
                return new SalesManager(Id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city);
            }
            return null;
        }

        private IEnumerable<SqlParameter> GetSqlParametersFromSkillset(Skillset skillset, bool withId)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Name", skillset.Name),
                new SqlParameter("@Description", skillset.Description)
            };
            if (withId)
            {
                parameters.Add(new SqlParameter("@Id", skillset.Id));
            }
            return parameters;
        }
    }
}
