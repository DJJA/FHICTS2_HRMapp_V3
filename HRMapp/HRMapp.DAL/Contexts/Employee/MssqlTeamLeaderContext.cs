using System;
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
                employees.AddRange(from DataRow row in dt.Rows select MssqlObjectFactory.GetTeamLeaderFromDataRow(row));
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
                    employee = MssqlObjectFactory.GetTeamLeaderFromDataRow(dt.Rows[0]);
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
                addedEmployee = ExecuteProcedureWithReturnValue("sp_AddTeamLeader", MssqlObjectFactory.GetSqlParametersFromTeamLeader(employee, false));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return addedEmployee;
        }

        public void Update(TeamLeader employee)
        {
            try
            {
                ExecuteProcedure("sp_UpdateTeamLeader", MssqlObjectFactory.GetSqlParametersFromTeamLeader(employee, true));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
        }


        public bool ChangeToThisTypeAndUpdate(TeamLeader employee)
        {
            try
            {
                ExecuteProcedure("sp_ChangeEmployeeTypeToTeamLeader", MssqlObjectFactory.GetSqlParametersFromTeamLeader(employee, true));
                return true;
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
                return false;
            }
        }

        public IEnumerable<ProductionWorker> GetTeamMembers(int teamLeaderId)
        {
            var teamMembers = new List<ProductionWorker>();

            try
            {
                var dataTable = GetDataViaProcedure("sp_GetTeamMembers", new SqlParameter("@TeamLeaderId", teamLeaderId));
                teamMembers.AddRange(from DataRow row in dataTable.Rows select MssqlObjectFactory.GetProductionWorkerFromDataRow(row));   // TODO Ik laad hier alle teammembers in met alle properties van hun, is dat nodig?
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }

            return teamMembers;
        }
    }
}
