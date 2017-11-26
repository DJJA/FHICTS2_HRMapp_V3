using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HRMapp.Models;
using System.Data.SqlClient;
using System.Linq;

namespace HRMapp.DAL.Contexts
{
    class MssqlOrderContext : MssqlDatabase, IOrderContext
    {
        public IEnumerable<Order> GetAll()
        {
            var orders = new List<Order>();
            try
            {
                var dt = GetDataViaProcedure("sp_GetOrders");
                orders.AddRange(from DataRow row in dt.Rows select GetOrderFromDataRow(row));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return orders;
        }

        public Order GetById(int id)
        {
            Order order = null;
            try
            {
                var dt = GetDataViaProcedure("sp_GetOrderById", new SqlParameter("@Id", id));
                if (dt.Rows.Count > 0)
                {
                    order = GetOrderFromDataRow(dt.Rows[0]);
                }
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return order;
        }

        public int Add(Order value)
        {
            int addedId = -1;
            try
            {
                addedId = ExecuteProcedureWithReturnValue("sp_AddOrder", GetSqlParametersFromOrder(value, false));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return addedId;
        }

        public bool Delete(Order value)
        {
            throw new NotImplementedException();
        }

        public bool Update(Order value)
        {
            try
            {
                var parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Deadline", value.DeadLine),
                    new SqlParameter("@Customer", value.Customer),
                    new SqlParameter("@Id", value.Id)
                };

                ExecuteProcedure("sp_UpdateOrder", parameters);
                return true;
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
                return false;
            }
        }

        private Order GetOrderFromDataRow(DataRow row)
        {
            var id = Convert.ToInt32(row["Id"]);
            var salesManagerId = Convert.ToInt32(row["EmployeeSalesManagerId"]);
            var deadline = Convert.ToDateTime(row["Deadline"]);
            var entryDate = Convert.ToDateTime(row["EntryDate"]);
            var customer = row["Customer"].ToString();
            return new Order(id, salesManagerId, deadline, entryDate, customer);
        }

        private IEnumerable<SqlParameter> GetSqlParametersFromOrder(Order order, bool withId)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeSalesManagerId", order.SalesManagerId), // todo If this is null, will it not pass this parameter to the db?
                new SqlParameter("@Deadline", order.DeadLine),
                new SqlParameter("@Customer", order.Customer)
            };
            if (withId)
            {
                parameters.Add(new SqlParameter("@Id", order.Id));
            }
            return parameters;
        }
    }
}
