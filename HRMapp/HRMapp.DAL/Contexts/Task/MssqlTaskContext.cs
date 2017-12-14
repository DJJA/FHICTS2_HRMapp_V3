using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Models;
using HRMapp.Models.Exceptions;

namespace HRMapp.DAL.Contexts
{
    internal class MssqlTaskContext : MssqlDatabase, ITaskContext
    {
        public IEnumerable<ProductionTask> GetAll()
        {
            var tasks = new List<ProductionTask>();

            try
            {
                var dataTable = GetDataViaProcedure("sp_GetTasks");
                tasks.AddRange(from DataRow row in dataTable.Rows select MssqlObjectFactory.GetTaskFromDataRow(row, GetEmployeesByTaskId(Convert.ToInt32(row["Id"]))));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }

            return tasks;
        }

        public ProductionTask GetById(int id)
        {
            ProductionTask task = null;

            try
            {
                var dataTable = GetDataViaProcedure("sp_GetTaskById", new SqlParameter("@Id", id));
                if (dataTable.Rows.Count > 0)
                {
                    task = MssqlObjectFactory.GetTaskFromDataRow(dataTable.Rows[0], GetEmployeesByTaskId(id));
                }
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }

            return task;
        }

        public int Add(ProductionTask task)
        {
            int addedTaskId = -1;
            try
            {
                addedTaskId = ExecuteProcedureWithReturnValue("sp_AddTask", MssqlObjectFactory.GetSqlParametersFromTask(task, false));
            }
            catch (SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2627: throw new DBException("Er bestaat al een taak met deze naam.");
                }
                HandleGenericSqlException(sqlEx);
            }
            
            return addedTaskId;
        }

        public void Delete(ProductionTask task)
        {
            try
            {
                ExecuteProcedure("sp_DeleteTaskById", new SqlParameter("@Id", task.Id));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
        }

        public void Update(ProductionTask task)
        {
            try
            {
                ExecuteProcedure("sp_UpdateTask", MssqlObjectFactory.GetSqlParametersFromTask(task, true));
            }
            catch (SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2627: throw new DBException("Er bestaat al een taak met deze naam.");
                }
                HandleGenericSqlException(sqlEx);
            }
        }

        public IEnumerable<ProductionTask> GetByProductId(int productId)
        {
            var tasks = new List<ProductionTask>();

            try
            {
                var dataTable = GetDataViaProcedure("sp_GetTasksByProductId", new SqlParameter("@ProductId", productId));
                tasks.AddRange(from DataRow row in dataTable.Rows select MssqlObjectFactory.GetTaskFromDataRow(row, GetEmployeesByTaskId(Convert.ToInt32(row["Id"]))));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }

            return tasks;
        }

        public static List<ProductionEmployee> GetEmployeesByTaskId(int taskId)// TODO dit is fucking lelijk, fix dit!
        {
            var employees = new List<ProductionEmployee>();

            try
            {
                var dataTable = new MssqlTaskContext().GetDataViaProcedure("sp_GetEmployeesByTaskId", new SqlParameter("@TaskId", taskId));
                employees.AddRange(from DataRow row in dataTable.Rows select new ProductionEmployee(
                                                                                            id: Convert.ToInt32(row["Id"]),
                                                                                            firstName: row["FirstName"].ToString(),
                                                                                            lastName: row["LastName"].ToString())
                                                                                            );
            }
            catch (SqlException sqlEx)
            {
                new MssqlTaskContext().HandleGenericSqlException(sqlEx);
            }

            return employees;
        }

    }
}
