using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HRMapp.Models;
using System.Data.SqlClient;
using System.Linq;

namespace HRMapp.DAL.Contexts
{
    public class MssqlSkillsetContext : MssqlDatabase, ISkillsetContext
    {
        public int Add(Skillset value)
        {
            int addedSkillset = -1;
            try
            {
                var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Name", value.Name),
                    new SqlParameter("@Description", value.Description)
                };
                addedSkillset = ExecuteProcedureWithReturnValue("sp_AddSkillset", parameters);
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

            try
            {
                var dt = GetDataViaProcedure("sp_GetSkillsets");
                skillsets.AddRange(from DataRow row in dt.Rows select GetSkillsetFromDataRow(row));
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

            try
            {
                var dt = GetDataViaProcedure("sp_GetSkillsetById", new SqlParameter("@Id", id));

                if (dt.Rows.Count > 0)
                {
                    skillset = GetSkillsetFromDataRow(dt.Rows[0]);
                }
            }
            catch (SqlException sqlEx)
            {
                
            }

            return skillset;
        }

        public bool Update(Skillset value)
        {
            try
            {
                var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Id", value.Id),
                    new SqlParameter("@Name", value.Name),
                    new SqlParameter("@Description", value.Description)
                };
                ExecuteProcedure("sp_UpdateSkillset", parameters);
                return true;
            }
            catch (SqlException sqlEx)
            {
                return false;
            }
        }

        private Skillset GetSkillsetFromDataRow(DataRow row)
        {
            var skillsetId = Convert.ToInt32(row["Id"]);
            var name = row["Name"].ToString();
            var descriptiom = row["Description"].ToString();
            return new Skillset(skillsetId, name, descriptiom);
        }




        //public void OutputParamTest()
        //{
        //    string output = string.Empty;

        //    string query = "sp_OutParamTest";

        //    try
        //    {
        //        using (var connection = new SqlConnection(connectionString))
        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            connection.Open();  // Scalar needs open connection
        //            command.CommandType = CommandType.StoredProcedure;

        //            command.Parameters.AddWithValue("@Id", 1);
        //            //command.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Direction = ParameterDirection.Output;
        //            var param = new SqlParameter("@Name", SqlDbType.VarChar, 30);
                    
        //            param.Direction = ParameterDirection.Output;
        //            command.Parameters.Add(param);

        //            var obj = command.ExecuteNonQuery();
        //            output = command.Parameters["@Name"].Value.ToString();
        //        }

        //    }
        //    catch (SqlException sqlEx)
        //    {

        //    }
        //}
    }
}
