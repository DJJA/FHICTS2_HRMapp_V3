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
                employees.AddRange(from DataRow row in dt.Rows select MssqlObjectFactory.GetSalesManagerFromDataRow(row));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
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
                    employee =  MssqlObjectFactory.GetSalesManagerFromDataRow(dt.Rows[0]);
                }
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return employee;
        }

        public int Add(SalesManager employee)
        {
            int addedEmployee = -1;
            try
            {
                addedEmployee = ExecuteProcedureWithReturnValue("sp_AddSalesManager", MssqlObjectFactory.GetSqlParametersFromSalesManager(employee, false));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return addedEmployee;
        }

        public void Update(SalesManager employee)
        {
            try
            {
                ExecuteProcedure("sp_UpdateSalesManager", MssqlObjectFactory.GetSqlParametersFromSalesManager(employee, true));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
        }


        public bool ChangeToThisTypeAndUpdate(SalesManager employee)
        {
            try
            {
                ExecuteProcedure("sp_ChangeEmployeeTypeToSalesManager", MssqlObjectFactory.GetSqlParametersFromSalesManager(employee, true));
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
