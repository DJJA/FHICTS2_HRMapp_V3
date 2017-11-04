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
    public class MssqlTaskContext : MssqlDatabase, ITaskContext
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

            }

            UpdateRequiredSkillsets(task);
            return addedTaskId;
        }

        public bool Delete(ProductionTask value)
        {
            string query = "DELETE FROM Task " +
                           "WHERE Id = @Id;";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Id", value.Id);

                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException sqlEx)
            {
                return false;
            }
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
                return false;
            }
        }

        public IEnumerable<Skillset> GetRequiredSkillsets(int taskId)
        {
            var skillsets = new List<Skillset>();
            string query = "SELECT * FROM Skillset " +
                           "WHERE Id IN" +
                           "(SELECT SkillsetId FROM Skillset_Task " +
                           "WHERE TaskId = @TaskId);";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var adapter = new SqlDataAdapter(query, connection))
                {
                    //connection.Open();
                    adapter.SelectCommand.Parameters.AddWithValue("@TaskId", taskId);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var row = dt.Rows[i];
                        var id = Convert.ToInt32(row["Id"]);
                        var name = row["Name"].ToString();
                        var description = row["Description"].ToString();
                        skillsets.Add(new Skillset(id, name, description));
                    }
                }
            }
            catch (SqlException sqlEx)
            {

            }

            return skillsets;
        }

        #region Update Task Skillset Links
        public bool UpdateRequiredSkillsets(ProductionTask task)
        {
            bool success = true;
            //throw new NotImplementedException();
            // Get all required skillsets that you need to add
            // Add these

            var taskInDb = GetById(task.Id);

            string query = "";
            foreach (var skillset in task.RequiredSkillsets)
            {
                if (taskInDb == null || taskInDb.RequiredSkillsets.All(s => s.Id != skillset.Id))
                {
                    AddSkillsetTaskLink(task, skillset);
                }
            }

            //foreach (var skillset in task.RequiredSkillsets)
            //{
            //    if (!AddSkillsetTaskLink(task, skillset))
            //    {
            //        success = false;
            //    }
            //}

            // Get all required skillsets that are not required anymore
            // Remove these

            // Parameters: taskId, [*]skillsetId
            if (!RemoveSkillsetTaskLinks(task))
            {
                success = false;
            }
            return success;
        }

        private bool AddSkillsetTaskLink(ProductionTask task, Skillset skillset)
        {
            string query = "INSERT INTO Skillset_Task (SkillsetId, TaskId)" +
                            "VALUES(@SkillsetId, @TaskId);";
            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@SkillsetId", skillset.Id);
                    command.Parameters.AddWithValue("@TaskId", task.Id);

                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException sqlEx)
            {
                return false;
            }
        }

        private bool RemoveSkillsetTaskLinks(ProductionTask task)
        {
            string query = "DELETE FROM Skillset_Task " +
                    "WHERE TaskId = @TaskId AND SkillsetId NOT IN(";



            for (int i = 0; i < task.RequiredSkillsets.Count; i++)
            {
                query += $"{(i > 0 ? "," : "")}@SkillsetId{i}";
            }

            query += ");";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@TaskId", task.Id);
                    for (int i = 0; i < task.RequiredSkillsets.Count; i++)
                    {
                        command.Parameters.AddWithValue($"@SkillsetId{i}", task.RequiredSkillsets[i].Id);
                    }

                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException sqlEx)
            {
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
