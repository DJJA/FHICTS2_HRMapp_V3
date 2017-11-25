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

            UpdateRequiredSkillsets(task);
            return addedTaskId;
        }

        public bool Delete(ProductionTask value)
        {
            throw new NotImplementedException();
        }

        public bool Update(ProductionTask task)
        {
            try
            {
                ExecuteProcedure("sp_UpdateTask", GetSqlParametersFromTask(task, true));

                UpdateRequiredSkillsets(task);

                return true;
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
                return false;
            }
        }

        #region Task Skillset Links
        public IEnumerable<Skillset> GetRequiredSkillsets(int taskId)
        {
            var skillsets = new List<Skillset>();

            try
            {
                var dataTable = GetDataViaProcedure("sp_GetRequiredSkillsets", new SqlParameter("@TaskId", taskId));
                skillsets.AddRange(from DataRow row in dataTable.Rows select MssqlSkillsetContext.GetSkillsetFromDataRow(row));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx); // TODO Moet ik hier al wel een specifieke error gooien
            }

            return skillsets;
        }

        
        public bool UpdateRequiredSkillsets(ProductionTask task)
        {
            var daaTablet = new DataTable();
            daaTablet.Columns.Add("Id");

            foreach (var skillset in task.RequiredSkillsets)
            {
                daaTablet.Rows.Add(skillset.Id);
            }

            var listWithRequiredSkillsetIds = new SqlParameter("@List", daaTablet)
            {
                SqlDbType = SqlDbType.Structured
            };

            var parameters = new List<SqlParameter>()
            {
                listWithRequiredSkillsetIds,
                new SqlParameter("@TaskId", task.Id)
            };

            try
            {
                ExecuteProcedure("sp_UpdateRequiredSkillsets", parameters);
                return true;
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
                return false;
            }
        }
        #endregion

        private ProductionTask GetTaskFromDataRow(DataRow row)
        {
            var id = Convert.ToInt32(row["Id"]);
            var name = row["Name"].ToString();
            var description = row["Description"].ToString();
            var duration = new TimeSpan(0, Convert.ToInt32(row["Duration"]), 0);
            var requiredSkillsets = GetRequiredSkillsets(id).ToList();
            return new ProductionTask(id, name, description, duration, requiredSkillsets);
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
            return parameters;
        }
    }
}
