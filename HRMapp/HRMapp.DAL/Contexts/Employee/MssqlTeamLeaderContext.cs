﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HRMapp.Models;
using System.Data.SqlClient;
using System.Linq;

namespace HRMapp.DAL.Contexts
{
    class MssqlTeamLeaderContext : MssqlDatabase, ITeamLeaderContext
    {
        public IEnumerable<TeamLeader> GetAll()
        {
            var employees = new List<TeamLeader>();
            try
            {
                var dt = GetDataViaProcedure("sp_GetTeamLeaders");
                employees.AddRange(from DataRow row in dt.Rows select GetTeamLeaderFromDataRow(row));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return employees;
        }

        public TeamLeader GetById(int id)
        {
            TeamLeader employee = null;
            try
            {
                var dt = GetDataViaProcedure("sp_GetTeamLeaderById", new SqlParameter("@Id", id));
                if (dt.Rows.Count > 0)
                {
                    employee = GetTeamLeaderFromDataRow(dt.Rows[0]);
                }
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return employee;
        }

        public int Add(TeamLeader employee)
        {
            int addedEmployee = -1;
            try
            {
                addedEmployee = ExecuteProcedureWithReturnValue("sp_AddTeamLeader", GetSqlParametersFromTeamLeader(employee, false));
                UpdateSkillsets(employee);
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return addedEmployee;
        }

        public bool Delete(TeamLeader value)
        {
            throw new NotImplementedException();
        }

        public bool Update(TeamLeader employee)
        {
            try
            {
                ExecuteProcedure("sp_UpdateTeamLeader", GetSqlParametersFromTeamLeader(employee, true));
                UpdateSkillsets(employee);
                return true;
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
                return false;
            }
        }

        private TeamLeader GetTeamLeaderFromDataRow(DataRow row)
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
            //var teamMembers = GetTeamMembers(id).ToList();  // TODO Won't work, infinite loop, get teammembers > get teamleader > get teamleaders > ect
            var teamMembers = new List<ProductionWorker>();
            return new TeamLeader(id, firstName, lastName, phoneNumber, emailAddress, street, houseNumber, zipCode, city, skillsets, teamMembers);
        }

        private IEnumerable<SqlParameter> GetSqlParametersFromTeamLeader(TeamLeader teamLeader, bool withId)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@FirstName", teamLeader.FirstName),
                new SqlParameter("@LastName", teamLeader.LastName),
                new SqlParameter("@PhoneNumber", teamLeader.PhoneNumber),
                new SqlParameter("@EmailAddress", teamLeader.EmailAddress),
                new SqlParameter("@Street", teamLeader.Street),
                new SqlParameter("@HouseNumber", teamLeader.HouseNumber),
                new SqlParameter("@ZipCode", teamLeader.ZipCode),
                new SqlParameter("@City", teamLeader.City)
            };
            if (withId)
            {
                parameters.Add(new SqlParameter("@Id", teamLeader.Id));
            }
            return parameters;
        }

        public bool ChangeToThisTypeAndUpdate(TeamLeader employee)
        {
            try
            {
                ExecuteProcedure("sp_ChangeEmployeeTypeToTeamLeader", GetSqlParametersFromTeamLeader(employee, true));
                return true;
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
                return false;
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
                HandleGenericSqlException(sqlEx);
            }

            return skillsets;
        }

        public void UpdateSkillsets(TeamLeader teamLeader)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Id");

            foreach (var skillset in teamLeader.Skillsets)
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
                new SqlParameter("@EmployeeId", teamLeader.Id)
            };

            try
            {
                ExecuteProcedure("sp_UpdateEmployeeSkillsets", parameters);
                //return true;
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
        }

        public IEnumerable<ProductionWorker> GetTeamMembers(int teamLeaderId)
        {
            var teamMembers = new List<ProductionWorker>();

            try
            {
                var dataTable = GetDataViaProcedure("sp_GetTeamMembers", new SqlParameter("@TeamLeaderId", teamLeaderId));
                teamMembers.AddRange(from DataRow row in dataTable.Rows select new MssqlProductionWorkerContext().GetProductionWorkerFromDataRow(row));   // TODO Ik laad hier alle teammembers in met alle properties van hun, is dat nodig?
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }

            return teamMembers;
        }
    }
}
