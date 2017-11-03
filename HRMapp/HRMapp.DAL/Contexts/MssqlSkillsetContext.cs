using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HRMapp.Models;
using System.Data.SqlClient;

namespace HRMapp.DAL.Contexts
{
    public class MssqlSkillsetContext : MssqlDatabase, ISkillsetContext
    {
        public int Add(Skillset value)
        {
            string query = "INSERT INTO Skillset (Name, Description)" +
                           "VALUES (@Name, @Description);" +
                           "SELECT SCOPE_IDENTITY();";
            int addedSkillset = -1;
            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Name", value.Name);
                    command.Parameters.AddWithValue("@Description", value.Description);

                    var obj = command.ExecuteScalar();
                    addedSkillset = Convert.ToInt32(obj);
                }
                
            }
            catch (SqlException sqlEx)
            {
                
            }
            return addedSkillset;
        }

        public bool Delete(Skillset value)
        {
            string query = "DELETE FROM Skillset " +
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

        public IEnumerable<Skillset> GetAll()
        {
            var skillsets = new List<Skillset>();
            string query = "SELECT * FROM Skillset;";

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
                        skillsets.Add(new Skillset(id, name, description));
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                //switch (sqlEx.Number)
                //{
                //    //case 4:
                //    //    throw new DBJobOpeningException("Nu ff deze error bestaat nog niet.");
                //    //    break;
                //}
            }

            return skillsets;
        }

        public Skillset GetById(int id)
        {
            Skillset skillset = null;
            string query = "SELECT * FROM Skillset WHERE Id = @Id;";

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
                        var skillsetId = Convert.ToInt32(row["Id"]);
                        var name = row["Name"].ToString();
                        var descriptiom = row["Description"].ToString();
                        skillset = new Skillset(skillsetId, name, descriptiom);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                
            }

            return skillset;
        }

        public bool Update(Skillset value)
        {
            string query = "UPDATE Skillset " +
                           "SET Name = @Name, Description = @Description " +
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

                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException sqlEx)
            {
                return false;
            }
        }
    }
}
