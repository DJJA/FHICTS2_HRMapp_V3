﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Models;

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
                tasks.AddRange(from DataRow row in dataTable.Rows select GetTaskFromDataRow(row));
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
                    task = GetTaskFromDataRow(dataTable.Rows[0]);
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
                addedTaskId = ExecuteProcedureWithReturnValue("sp_AddTask", GetSqlParametersFromTask(task, false));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            
            return addedTaskId;
        }

        public bool Delete(ProductionTask task)
        {
            try
            {
                ExecuteProcedure("sp_DeleteTaskById", new SqlParameter("@Id", task.Id));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return false;
        }

        public bool Update(ProductionTask task)
        {
            try
            {
                ExecuteProcedure("sp_UpdateTask", GetSqlParametersFromTask(task, true));
                return true;
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
                return false;
            }
        }

        public IEnumerable<ProductionTask> GetByProductId(int productId)
        {
            var tasks = new List<ProductionTask>();

            try
            {
                var dataTable = GetDataViaProcedure("sp_GetTasksByProductId", new SqlParameter("@ProductId", productId));
                tasks.AddRange(from DataRow row in dataTable.Rows select GetTaskFromDataRow(row));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }

            return tasks;
        }

        private static List<Employee> GetEmployeesByTaskId(int taskId)// TODO dit is fucking lelijk, fix dit!
        {
            var employees = new List<Employee>();

            try
            {
                var dataTable = new MssqlTaskContext().GetDataViaProcedure("sp_GetEmployeesByTaskId", new SqlParameter("@TaskId", taskId));
                employees.AddRange(from DataRow row in dataTable.Rows select new Employee(
                                                                                            id: Convert.ToInt32(row["Id"]),
                                                                                            firstName: row["FirstName"].ToString(),
                                                                                            lastName: row["LastName"].ToString()));
            }
            catch (SqlException sqlEx)
            {
                new MssqlTaskContext().HandleGenericSqlException(sqlEx);
            }

            return employees;
        }

        public static ProductionTask GetTaskFromDataRow(DataRow row)
        {
            var id = Convert.ToInt32(row["Id"]);
            var productId = Convert.ToInt32(row["ProductId"]);
            var name = row["Name"].ToString();
            var description = row["Description"].ToString();
            var duration = new TimeSpan(0, Convert.ToInt32(row["Duration"]), 0);
            var employees = GetEmployeesByTaskId(id);// TODO dit is fucking lelijk, fix dit!
            return new ProductionTask(id, new Product(productId), name, description, duration, employees);
        }

        private IEnumerable<SqlParameter> GetSqlParametersFromTask(ProductionTask task, bool withId)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Name", task.Name),
                new SqlParameter("@Description", task.Description),
                new SqlParameter("@Duration", (task.Duration.Hours * 60) + task.Duration.Minutes),
            };
            if (withId)
            {
                parameters.Add(new SqlParameter("@Id", task.Id));
            }
            else
            {
                parameters.Add(new SqlParameter("@ProductId", task.Product.Id));
            }
            return parameters;
        }
    }
}
