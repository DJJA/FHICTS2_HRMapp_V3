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
            string query = "SELECT * FROM Task;";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var row = dt.Rows[i];
                        var id = Convert.ToInt32(row["Id"]);
                        var name = row["Name"].ToString();
                        var description = row["Description"].ToString();
                        var duration = new TimeSpan(0, Convert.ToInt32(row["Duration"]), 0);
                        var requiredSkillsets = GetRequiredSkillsets(id).ToList();
                        tasks.Add(new ProductionTask(id, name, description, duration, requiredSkillsets));
                    }
                }
            }
            catch (SqlException sqlEx)
            {

            }

            return tasks;
        }

        public ProductionTask GetById(int id)
        {
            ProductionTask task = null;
            string query = "SELECT * FROM Task WHERE Id = @Id;";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var adapter = new SqlDataAdapter(query, connection))
                {
                    connection.Open();
                    adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        var row = dt.Rows[0];
                        var taskId = Convert.ToInt32(row["Id"]);
                        var name = row["Name"].ToString();
                        var description = row["Description"].ToString();
                        var duration = new TimeSpan(0, Convert.ToInt32(row["Duration"]), 0);
                        var requiredSkillsets = GetRequiredSkillsets(taskId).ToList();
                        task = new ProductionTask(taskId, name, description, duration, requiredSkillsets);
                    }
                }
            }
            catch (SqlException sqlEx)
            {

            }

            return task;
        }

        public int Add(ProductionTask value)
        {
            string query = "INSERT INTO Task (Name, Description, Duration)" +
                           "VALUES (@Name, @Description, @Duration);" +
                           "SELECT SCOPE_IDENTITY();";
            int addedTask = -1;
            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Name", value.Name);
                    command.Parameters.AddWithValue("@Description", value.Description);
                    command.Parameters.AddWithValue("@Duration", (value.Duration.Hours * 60) + value.Duration.Minutes);

                    var obj = command.ExecuteScalar();
                    addedTask = Convert.ToInt32(obj);
                }

            }
            catch (SqlException sqlEx)
            {

            }
            return addedTask;
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

        public bool Update(ProductionTask value)
        {
            string query = "UPDATE Task " +
                           "SET Name = @Name, Description = @Description, Duration = @Duration " +
                           "WHERE Id = @Id;";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Id", value.Id);
                    command.Parameters.AddWithValue("@Name", value.Name);
                    command.Parameters.AddWithValue("@Description", value.Description);
                    command.Parameters.AddWithValue("@Duration", (value.Duration.Hours * 60) + value.Duration.Minutes);

                    command.ExecuteNonQuery();
                }
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

        public bool UpdateRequiredSkillsets(ProductionTask task)
        {
            throw new NotImplementedException();
            // Get all required skillsets that you need to add
            // Add these
            // Get all required skillsets that are not required anymore
            // Remove these

            // Parameters: taskId, [*]skillsetId
            string query = "DELETE FROM Skillset_Task " +
                           "WHERE TaskId = @TaskId;";
            
        }
    }
}
