using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRMapp.DAL
{
    public abstract class MssqlDatabase
    {
        protected readonly string connectionString = "Server=tcp:djjaserver.database.windows.net,1433;Initial Catalog=HRMapp;Persist Security Info=False;User ID=djja;Password=DrEh437u;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
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
                    foreach (var parammeter in procedureParameters)
                    {
                        adapter.SelectCommand.Parameters.Add(parammeter);
                    }
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
                connection.Open();  // ExecuteNonQuery requires open connection
                command.CommandType = CommandType.StoredProcedure;

                if (procedureParameters != null)
                {
                    foreach (var parammeter in procedureParameters)
                    {
                        command.Parameters.Add(parammeter);
                    }
                }

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
                connection.Open();  // Scalar needs open connection
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(procedureParameters.ToArray());

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
                connection.Open();  // Scalar needs open connection
                command.CommandType = CommandType.StoredProcedure;

                if (procedureParameters != null)
                {
                    foreach (var parammeter in procedureParameters)
                    {
                        command.Parameters.Add(parammeter);
                    }
                }

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
    }
}
