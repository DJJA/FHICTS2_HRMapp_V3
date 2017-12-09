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

        public void Update(Order value)
        {
            try
            {
                ExecuteProcedure("sp_UpdateOrder", GetSqlParametersFromOrder(value, true));
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

        private Order GetOrderFromDataRow(DataRow row)
        {
            var id = Convert.ToInt32(row["Id"]);
            var salesManagerId = Convert.ToInt32(row["EmployeeSalesManagerId"]);
            var deadline = Convert.ToDateTime(row["Deadline"]);
            var entryDate = Convert.ToDateTime(row["EntryDate"]);
            var customer = row["Customer"].ToString();
            return new Order(
                id: id, 
                salesManager: new SalesManager(salesManagerId), 
                deadline: deadline, 
                entryDate:entryDate, 
                customer:customer,
                items: GetOrderItems(id)
                );
        }

        private IEnumerable<SqlParameter> GetSqlParametersFromOrder(Order order, bool withId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ProductId");
            dataTable.Columns.Add("Amount");
            foreach (var item in order.Items)
            {
                dataTable.Rows.Add(item.Product.Id, item.Amount);
            }

            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Deadline", order.Deadline),
                new SqlParameter("@Customer", order.Customer),
                new SqlParameter("@OrderItems", dataTable)
                {
                    SqlDbType = SqlDbType.Structured
                }
            };
            if (withId)
            {
                parameters.Add(new SqlParameter("@Id", order.Id));
            }
            else
            {
                parameters.Add(new SqlParameter("@EmployeeSalesManagerId", order.SalesManager.Id)); 
            }
            return parameters;
        }
    }
}
