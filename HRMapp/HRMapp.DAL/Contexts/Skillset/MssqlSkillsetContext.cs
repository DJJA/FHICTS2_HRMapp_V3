using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HRMapp.Models;
using System.Data.SqlClient;
using System.Linq;
using HRMapp.Models.Exceptions;

namespace HRMapp.DAL.Contexts
{
    internal class MssqlSkillsetContext : MssqlDatabase, ISkillsetContext
    {
        /// <summary>
        /// Adds new skillset to the system
        /// </summary>
        /// <param name="skillset"></param>
        /// <returns>The index of the newly added skillset</returns>
        public int Add(Skillset skillset)
        {
            int addedSkillset = -1;
            try
            {
                addedSkillset = ExecuteProcedureWithReturnValue("sp_AddSkillset", GetSqlParametersFromSkillset(skillset, false));
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
            return addedSkillset;
        }

        public bool Delete(Skillset value)
        {
            throw new NotImplementedException();
            //string query = "DELETE FROM Skillset " +
            //               "WHERE Id = @Id;";

            //try
            //{
            //    using (var connection = new SqlConnection(connectionString))
            //    using (var command = new SqlCommand(query, connection))
            //    {
            //        connection.Open();
            //        command.Parameters.AddWithValue("@Id", value.Id);

            //        command.ExecuteNonQuery();
            //    }
            //    return true;
            //}
            //catch (SqlException sqlEx)
            //{
            //    return false;
            //}
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
                throw HandleGenericSqlException(sqlEx);
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
                throw HandleGenericSqlException(sqlEx);
            }
            return skillset;
        }

        public bool Update(Skillset skillset)
        {
            try
            {
                ExecuteProcedure("sp_UpdateSkillset", GetSqlParametersFromSkillset(skillset, true));
                return true;
            }
            catch (SqlException sqlEx)
            {
                throw HandleGenericSqlException(sqlEx);
            }
        }

        internal static Skillset GetSkillsetFromDataRow(DataRow row)
        {
            var skillsetId = Convert.ToInt32(row["Id"]);
            var name = row["Name"].ToString();
            var descriptiom = row["Description"].ToString();
            return new Skillset(skillsetId, name, descriptiom);
        }

        private IEnumerable<SqlParameter> GetSqlParametersFromSkillset(Skillset skillset, bool withId)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Name", skillset.Name),
                new SqlParameter("@Description", skillset.Description)
            };
            if (withId)
            {
                parameters.Add(new SqlParameter("@Id", skillset.Id));
            }
            return parameters;
        }
    }
}
