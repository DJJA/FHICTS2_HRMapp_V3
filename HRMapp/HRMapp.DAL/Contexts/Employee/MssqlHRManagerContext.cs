using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HRMapp.Models;

namespace HRMapp.DAL.Contexts
{
    class MssqlHRManagerContext : MssqlDatabase, IHRManagerContext
    {
        public IEnumerable<HRManager> GetAll()
        {
            var employees = new List<HRManager>();
            try
            {
                var dt = GetDataViaProcedure("sp_GetHRManagers");
                employees.AddRange(from DataRow row in dt.Rows select MssqlObjectFactory.GetHRManagerFromDataRow(row));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
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
                    employee = MssqlObjectFactory.GetHRManagerFromDataRow(dt.Rows[0]);
                }
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return employee;
        }

        public int Add(HRManager employee)
        {
            int addedEmployee = -1;
            try
            {
                addedEmployee = ExecuteProcedureWithReturnValue("sp_AddHRManager", MssqlObjectFactory.GetSqlParametersFromHRManager(employee, false));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return addedEmployee;
        }

        public void Update(HRManager employee)
        {
            try
            {
                ExecuteProcedure("sp_UpdateHRManager", MssqlObjectFactory.GetSqlParametersFromHRManager(employee, true));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
        }


        public bool ChangeToThisTypeAndUpdate(HRManager employee)
        {
            try
            {
                ExecuteProcedure("sp_ChangeEmployeeTypeToHRManager", MssqlObjectFactory.GetSqlParametersFromHRManager(employee, true));
                return true;
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
                return false;
            }
        }
    }
}
