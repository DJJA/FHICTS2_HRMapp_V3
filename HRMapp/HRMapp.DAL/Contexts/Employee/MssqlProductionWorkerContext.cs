using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    class MssqlProductionWorkerContext : MssqlDatabase, IProductionWorkerContext
    {
        public IEnumerable<ProductionWorker> GetAll()
        {
            var employees = new List<ProductionWorker>();
            try
            {
                var dt = GetDataViaProcedure("sp_GetProductionWorkers");
                employees.AddRange(from DataRow row in dt.Rows select GetProductionWorkerFromDataRow(row));
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
            return employees;
        }

        public ProductionWorker GetById(int id)
        {
            ProductionWorker employee = null;
            try
            {
                var dt = GetDataViaProcedure("sp_GetProductionWorkerById", new SqlParameter("@Id", id));
                if (dt.Rows.Count > 0)
                {
                    employee = GetProductionWorkerFromDataRow(dt.Rows[0]);
                }
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
            return employee;
        }

        public int Add(ProductionWorker employee)
        {
            int addedEmployee = -1;
            try
            {
                addedEmployee = ExecuteProcedureWithReturnValue("sp_AddProductionWorker", GetSqlParametersFromProductionWorker(employee, false));
                UpdateSkillsets(employee);
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
            return addedEmployee;
        }

        public bool Delete(ProductionWorker value)
        {
            throw new NotImplementedException();
        }

        public bool Update(ProductionWorker employee)
        {
            try
            {
                ExecuteProcedure("sp_UpdateProductionWorker", GetSqlParametersFromProductionWorker(employee, true));
                UpdateSkillsets(employee);
                return true;
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
        }

        public ProductionWorker GetProductionWorkerFromDataRow(DataRow row)
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

            var skillsets = GetSkillsets(id).ToList();
            TeamLeader teamLeader = null;
            if (row["TeamLeaderId"] != DBNull.Value)
            {
                var teamLeaderId = Convert.ToInt32(row["TeamLeaderId"]);
                teamLeader = new EmployeeRepo().GetTeamLeaderById(teamLeaderId);
            }
            
            return new ProductionWorker(id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city, skillsets, teamLeader);
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
            if (productionWorker.TeamLeader == null)
            {
                parameters.Add(new SqlParameter("@TeamLeaderId", DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@TeamLeaderId", productionWorker.TeamLeader.Id));
            }
            if (withId)
            {
                parameters.Add(new SqlParameter("@Id", productionWorker.Id));
            }
            foreach (var parameter in parameters)
            {
                if (parameter.Value == null) parameter.Value = DBNull.Value;
            }
            return parameters;
        }

        public bool ChangeToThisTypeAndUpdate(ProductionWorker employee)
        {
            try
            {
                ExecuteProcedure("sp_ChangeEmployeeTypeToProductionWorker", GetSqlParametersFromProductionWorker(employee, true));
                return true;
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
        }

        public IEnumerable<Skillset> GetSkillsets(int employeeId)
        {
            var skillsets = new List<Skillset>();

            try
            {
                var dataTable = GetDataViaProcedure("sp_GetEmployeeSkillsets", new SqlParameter("@EmployeeId", employeeId));
                skillsets.AddRange(from DataRow row in dataTable.Rows select MssqlSkillsetContext.GetSkillsetFromDataRow(row)); // TODO mssqlskillsetcontext aanroepen hier is misschien niet zo netjes
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }

            return skillsets;
        }

        public void UpdateSkillsets(ProductionWorker productionWorker)  // TODO Exact hetzelfde als in TeamLeaderContext
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Id");

            foreach (var skillset in productionWorker.Skillsets)
            {
                dataTable.Rows.Add(skillset.Id);
            }

            var listWithRequiredSkillsetIds = new SqlParameter("@List", dataTable)
            {
                SqlDbType = SqlDbType.Structured
            };

            var parameters = new List<SqlParameter>()
            {
                listWithRequiredSkillsetIds,
                new SqlParameter("@EmployeeId", productionWorker.Id)
            };

            try
            {
                ExecuteProcedure("sp_UpdateEmployeeSkillsets", parameters);
                //return true;
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
        }
    }
}
