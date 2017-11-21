using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    class MssqlSalesManagerContext : MssqlDatabase, ISalesManagerContext
    {
        public IEnumerable<SalesManager> GetAll()
        {
            var employees = new List<SalesManager>();
            try
            {
                var dt = GetDataViaProcedure("sp_GetSalesManagers");
                employees.AddRange(from DataRow row in dt.Rows select GetSalesManagerFromDataRow(row));
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
            return employees;
        }

        public SalesManager GetById(int id)
        {
            SalesManager employee = null;
            try
            {
                var dt = GetDataViaProcedure("sp_GetSalesManagerById", new SqlParameter("@Id", id));
                if (dt.Rows.Count > 0)
                {
                    employee = GetSalesManagerFromDataRow(dt.Rows[0]);
                }
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
            return employee;
        }

        public int Add(SalesManager employee)
        {
            int addedEmployee = -1;
            try
            {
                addedEmployee = ExecuteProcedureWithReturnValue("sp_AddSalesManager", GetSqlParametersFromSalesManager(employee, false));
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
            return addedEmployee;
        }

        public bool Delete(SalesManager value)
        {
            throw new NotImplementedException();
        }

        public bool Update(SalesManager employee)
        {
            try
            {
                ExecuteProcedure("sp_UpdateSalesManager", GetSqlParametersFromSalesManager(employee, true));
                return true;
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
        }

        private SalesManager GetSalesManagerFromDataRow(DataRow row)
        {
            var id = Convert.ToInt32(row["EmployeeId"]);
            var firstName = row["FirstName"].ToString();
            var lastName = row["LastName"].ToString();
            var phoneNumber = row["PhoneNumber"].ToString();
            var emailAddress = row["EmailAddress"].ToString();
            var street = row["Street"].ToString();
            var houseNumber = row["HouseNumber"].ToString();
            var zipCode = row["ZipCode"].ToString();
            var city = row["City"].ToString();

            return new SalesManager(id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city);
        }

        private IEnumerable<SqlParameter> GetSqlParametersFromSalesManager(SalesManager hrManager, bool withId)
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

        public bool ChangeToThisTypeAndUpdate(SalesManager employee)
        {
            try
            {
                ExecuteProcedure("sp_ChangeEmployeeTypeToSalesManager", GetSqlParametersFromSalesManager(employee, true));
                return true;
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
        }
    }
}
