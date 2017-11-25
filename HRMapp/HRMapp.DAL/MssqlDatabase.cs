using HRMapp.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRMapp.DAL
{
    internal abstract class MssqlDatabase
    {
        //private readonly string connectionString = "Server=tcp:djjaserver.database.windows.net,1433;Initial Catalog=HRMapp;Persist Security Info=False;User ID=djja;Password=DrEh437u;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private const string connectionString = @"Server=localhost\SQLEXPRESS;Database=HRMapp;Trusted_Connection=True;";

        #region GetDataViaProcedure
        protected DataTable GetDataViaProcedure(string procedure, IEnumerable<SqlParameter> procedureParameters)
        {
            var datatable = new DataTable();
            using (var connection = new SqlConnection(connectionString))
            using (var adapter = new SqlDataAdapter(procedure, connection))
            {
                //connection.Open();
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                if (procedureParameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(ValidateParameters(procedureParameters).ToArray());
                    //foreach (var parammeter in procedureParameters)
                    //{
                    //    adapter.SelectCommand.Parameters.Add(parammeter);
                    //}
                }

                adapter.Fill(datatable);
            }
            return datatable;
        }

        protected DataTable GetDataViaProcedure(string procedure)
        {
            return GetDataViaProcedure(procedure, procedureParameters: null);
        }

        protected DataTable GetDataViaProcedure(string procedure, SqlParameter procedureParameter)
        {
            return GetDataViaProcedure(procedure, new List<SqlParameter>() { procedureParameter });
        }
        #endregion
        #region ExecuteProcedure
        protected void ExecuteProcedure(string procedure, IEnumerable<SqlParameter> procedureParameters)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(procedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (procedureParameters != null)
                {
                    command.Parameters.AddRange(ValidateParameters(procedureParameters).ToArray());
                    //foreach (var parammeter in procedureParameters)
                    //{
                    //    command.Parameters.Add(parammeter);
                    //}
                }
                connection.Open();  // ExecuteNonQuery requires open connection

                command.ExecuteNonQuery();
            }
        }

        protected void ExecuteProcedure(string procedure)
        {
            ExecuteProcedure(procedure, procedureParameters: null);
        }

        protected void ExecuteProcedure(string procedure, SqlParameter procedureParameter)
        {
            ExecuteProcedure(procedure, new List<SqlParameter>() { procedureParameter });
        }
        #endregion
        #region ExecuteProcedureWithOutputParameters
        protected List<SqlParameter> ExecuteProcedureWithOutputParameters(string procedure, List<SqlParameter> procedureParameters)
        {
            var outputParameters = new List<SqlParameter>();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(procedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(ValidateParameters(procedureParameters).ToArray());
                connection.Open();  // Scalar needs open connection

                command.ExecuteNonQuery();
                outputParameters.AddRange(command.Parameters.Cast<SqlParameter>());
            }

            return outputParameters;
        }
        #endregion
        #region ExecuteProcedureWithReturnValue
        protected int ExecuteProcedureWithReturnValue(string procedure, IEnumerable<SqlParameter> procedureParameters)
        {
            int returnValue = -1;

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(procedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (procedureParameters != null)
                {
                    command.Parameters.AddRange(ValidateParameters(procedureParameters).ToArray());
                    //foreach (var parammeter in procedureParameters)
                    //{
                    //    command.Parameters.Add(parammeter);
                    //}
                }
                connection.Open();  // Scalar needs open connection

                var obj = command.ExecuteScalar();
                returnValue = Convert.ToInt32(obj);
            }

            return returnValue;
        }

        protected int ExecuteProcedureWithReturnValue(string procedure)
        {
            return ExecuteProcedureWithReturnValue(procedure, procedureParameters: null);
        }

        protected int ExecuteProcedureWithReturnValue(string procedure, SqlParameter procedureParameter)
        {
            return ExecuteProcedureWithReturnValue(procedure, new List<SqlParameter>() { procedureParameter });
        }
        #endregion

        #region OutputParamTest Example
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
        #endregion

        protected void HandleGenericSqlException(SqlException sqlEx)
        {
            switch (sqlEx.Number)
            {
                case 11001: throw new DBException("Kan geen verbinding maken met de server.");
                default: throw new DBException($"Er is iets mis gegaan. {Environment.NewLine} ({sqlEx.Number}){sqlEx.Message}");
            }
        }

        private IEnumerable<SqlParameter> ValidateParameters(IEnumerable<SqlParameter> parameters)  // TODO Maybe not so efficient
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Value == null) parameter.Value = DBNull.Value;
            }
            return parameters;
        }
    }
}
