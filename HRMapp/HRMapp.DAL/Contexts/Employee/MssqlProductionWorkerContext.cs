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
                employees.AddRange(from DataRow row in dt.Rows select MssqlObjectFactory.GetProductionWorkerFromDataRow(row));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
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
                    employee = MssqlObjectFactory.GetProductionWorkerFromDataRow(dt.Rows[0]);
                }
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return employee;
        }

        public int Add(ProductionWorker employee)
        {
            int addedEmployee = -1;
            try
            {
                addedEmployee = ExecuteProcedureWithReturnValue("sp_AddProductionWorker", MssqlObjectFactory.GetSqlParametersFromProductionWorker(employee, false));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return addedEmployee;
        }

        public void Update(ProductionWorker employee)
        {
            try
            {
                ExecuteProcedure("sp_UpdateProductionWorker", MssqlObjectFactory.GetSqlParametersFromProductionWorker(employee, true));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
        }


        public bool ChangeToThisTypeAndUpdate(ProductionWorker employee)
        {
            try
            {
                ExecuteProcedure("sp_ChangeEmployeeTypeToProductionWorker", MssqlObjectFactory.GetSqlParametersFromProductionWorker(employee, true));
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
