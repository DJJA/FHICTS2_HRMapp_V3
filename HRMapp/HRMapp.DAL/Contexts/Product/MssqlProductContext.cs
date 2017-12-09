using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Models;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using HRMapp.Models.Exceptions;

namespace HRMapp.DAL.Contexts
{
    class MssqlProductContext : MssqlDatabase, IProductContext
    {
        public IEnumerable<Product> GetAll()
        {
            var products = new List<Product>();
            try
            {
                var dt = GetDataViaProcedure("sp_GetProducts");
                products.AddRange(from DataRow row in dt.Rows select GetProductFromDataRow(row));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return products;
        }

        public Product GetById(int id)
        {
            Product product = null;
            try
            {
                var dt = GetDataViaProcedure("sp_GetProductById", new SqlParameter("@Id", id));
                if (dt.Rows.Count > 0)
                {
                    product = GetProductFromDataRow(dt.Rows[0]);
                }
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
            return product;
        }

        public int Add(Product product)
        {
            int addedProduct = -1;
            try
            {
                addedProduct = ExecuteProcedureWithReturnValue("sp_AddProduct", GetSqlParametersFromProduct(product, false));
            }
            catch (SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2627: throw new DBException($"Product met naam '{product.Name}' bestaat al. Kies een andere naam.");   
                }
                HandleGenericSqlException(sqlEx);
            }
            return addedProduct;
        }

        public void Update(Product product)
        {
            try
            {
                ExecuteProcedure("sp_UpdateProduct", GetSqlParametersFromProduct(product, true));
            }
            catch (SqlException sqlEx)
            {
                HandleGenericSqlException(sqlEx);
            }
        }

        public IEnumerable<ProductionTask> GetRequiredTasks(int productId)
        {
            var tasks = new List<ProductionTask>();
            var dataTable = GetDataViaProcedure("sp_GetTasksByProductId", new SqlParameter("@ProductId", productId));
            tasks.AddRange(from DataRow row in dataTable.Rows select MssqlTaskContext.GetTaskFromDataRow(row));   
            return tasks;
        }

        private Product GetProductFromDataRow(DataRow row)
        {
            var id = Convert.ToInt32(row["Id"]);
            var name = row["Name"].ToString();
            var description = row["Description"].ToString();

            return new Product(
                id: id,
                name: name,
                description: description,
                tasks: GetRequiredTasks(id).ToList());
        }

        private IEnumerable<SqlParameter> GetSqlParametersFromProduct(Product product, bool withId)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Description", product.Description)   // TODO Wat als ie null is? zet null standaard om naar dbnull.value
            };
            if (withId)
            {
                parameters.Add(new SqlParameter("@Id", product.Id));
            }
            return parameters;
        }
    }
}
