using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    class MssqlHRManagerContext : MssqlDatabase, IHRManager
    {
        public IEnumerable<HRManager> GetAll()
        {
            var employees = new List<HRManager>();
            try
            {
                var dt = GetDataViaProcedure("sp_GetHRManagers");
                employees.AddRange(from DataRow row in dt.Rows select GetHRManagerFromDataRow(row));
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
            return employees;
        }

        public HRManager GetById(int id)
        {
            HRManager employee = null;
            try
            {
                var dt = GetDataViaProcedure("sp_GetHRManagerById", new SqlParameter("@Id", id));
                if (dt.Rows.Count > 0)
                {
                    employee = GetHRManagerFromDataRow(dt.Rows[0]);
                }
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
            return employee;
        }

        public int Add(HRManager employee)
        {
            int addedEmployee = -1;
            try
            {
                addedEmployee = ExecuteProcedureWithReturnValue("sp_AddHRManager", GetSqlParametersFromHRManager(employee, false));
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
            return addedEmployee;
        }

        public bool Delete(HRManager value)
        {
            throw new NotImplementedException();
        }

        public bool Update(HRManager employee)
        {
            try
            {
                ExecuteProcedure("sp_UpdateHRManager", GetSqlParametersFromHRManager(employee, true));
                return true;
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
        }

        private HRManager GetHRManagerFromDataRow(DataRow row)
        {
            var id = Convert.ToInt32(row["EmployeeId"]);
            var firstName = row["FirstName"].ToString();
            var lastName = row["LastName"].ToString();
            var phoneNumber = Convert.ToInt64(row["PhoneNumber"]);
            var emailAddress = row["EmailAddress"].ToString();
            var street = row["Street"].ToString();
            var houseNumber = row["HouseNumber"].ToString();
            var zipCode = row["ZipCode"].ToString();
            var city = row["City"].ToString();

            return new HRManager(id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city);
        }

        private IEnumerable<SqlParameter> GetSqlParametersFromHRManager(HRManager hrManager, bool withId)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@FirstName", hrManager.FirstName),
                new SqlParameter("@LastName", hrManager.LastName),
                new SqlParameter("@PhoneNumber", hrManager.PhoneNumber),
                new SqlParameter("@EmailAddress", hrManager.EmailAddress),
                new SqlParameter("@Street", hrManager.Street),
                new SqlParameter("@HouseNumber", hrManager.HouseNumber),
                new SqlParameter("@ZipCode", hrManager.ZipCode),
                new SqlParameter("@City", hrManager.City)
            };
            if (withId)
            {
                parameters.Add(new SqlParameter("@Id", hrManager.Id));
            }
            return parameters;
        }
    }
}
