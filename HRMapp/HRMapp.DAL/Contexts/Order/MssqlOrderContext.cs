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
                orders.AddRange(from DataRow row in dt.Rows select MssqlObjectFactory.GetOrderFromDataRow(row, GetOrderItems(Convert.ToInt32(row["Id"])))); // TODO Fix deze quik 'n dirty oplossing en in andere context classes
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
                    order = MssqlObjectFactory.GetOrderFromDataRow(dt.Rows[0], GetOrderItems(id));
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
                addedId = ExecuteProcedureWithReturnValue("sp_AddOrder", MssqlObjectFactory.GetSqlParametersFromOrder(value, false));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return addedId;
        }

        public void Update(Order value)
        {
            try
            {
                ExecuteProcedure("sp_UpdateOrder", MssqlObjectFactory.GetSqlParametersFromOrder(value, true));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
        }

        public List<OrderItem> GetOrderItems(int orderId)
        {
            var orderItems = new List<OrderItem>();
            var dataTable = GetDataByFunction("fn_GetOrderItems", orderId);
            orderItems.AddRange(from DataRow row in dataTable.Rows select new OrderItem()
            {
                Product = new Product(Convert.ToInt32(row["ProductId"]), row["Name"].ToString()),
                Amount = Convert.ToInt32(row["Amount"])
            });
            return orderItems;
        }

    }
}
