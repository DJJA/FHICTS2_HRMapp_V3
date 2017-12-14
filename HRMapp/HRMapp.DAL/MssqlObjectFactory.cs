using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Models;

namespace HRMapp.DAL
{
    static class MssqlObjectFactory
    {
        #region Task
        public static ProductionTask GetTaskFromDataRow(DataRow row, List<ProductionEmployee> employees)
        {
            var id = Convert.ToInt32(row["Id"]);
            var productId = Convert.ToInt32(row["ProductId"]);
            var name = row["Name"].ToString();
            var description = row["Description"].ToString();
            var duration = new TimeSpan(0, Convert.ToInt32(row["Duration"]), 0);
            //var employees = GetEmployeesByTaskId(id);// TODO dit is fucking lelijk, fix dit!
            return new ProductionTask(
                id: id,
                product: new Product(productId),
                name: name,
                description: description,
                duration: duration,
                employees: employees
                );
        }

        public static IEnumerable<SqlParameter> GetSqlParametersFromTask(ProductionTask task, bool withId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Id");
            foreach (var employee in task.Employees)
            {
                dataTable.Rows.Add(employee.Id);
            }

            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Name", task.Name),
                new SqlParameter("@Description", task.Description),
                new SqlParameter("@Duration", (task.Duration.Hours * 60) + task.Duration.Minutes),
                new SqlParameter("@QualifiedEmployeeIds", dataTable)
                {
                    SqlDbType = SqlDbType.Structured
                }
            };
            if (withId)
            {
                parameters.Add(new SqlParameter("@Id", task.Id));
            }
            else
            {
                parameters.Add(new SqlParameter("@ProductId", task.Product.Id));
            }
            return parameters;
        }
        #endregion
        #region Product
        public static Product GetProductFromDataRow(DataRow row, List<ProductionTask> requiredTasks)
        {
            var id = Convert.ToInt32(row["Id"]);
            var name = row["Name"].ToString();
            var description = row["Description"].ToString();

            return new Product(
                id: id,
                name: name,
                description: description,
                //tasks: GetRequiredTasks(id).ToList()
                tasks: requiredTasks
                );
        }

        public static IEnumerable<SqlParameter> GetSqlParametersFromProduct(Product product, bool withId)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Description", product.Description)
            };
            if (withId)
            {
                parameters.Add(new SqlParameter("@Id", product.Id));
            }
            return parameters;
        }
        #endregion
        #region Order
        public static Order GetOrderFromDataRow(DataRow row, List<OrderItem> orderItems)
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
                //items: GetOrderItems(id)
                items: orderItems
                );
        }

        public static IEnumerable<SqlParameter> GetSqlParametersFromOrder(Order order, bool withId)
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
        #endregion
        #region Employee
        private static List<SqlParameter> GetEmployeeParameters(Employee employee, bool withId)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@PhoneNumber", employee.PhoneNumber),
                new SqlParameter("@EmailAddress", employee.EmailAddress),
                new SqlParameter("@Street", employee.Street),
                new SqlParameter("@HouseNumber", employee.HouseNumber),
                new SqlParameter("@ZipCode", employee.ZipCode),
                new SqlParameter("@City", employee.City)
            };
            if (withId)
            {
                parameters.Add(new SqlParameter("@Id", employee.Id));
            }
            return parameters;
        }

        public static TeamLeader GetTeamLeaderFromDataRow(DataRow row)
        {
            var id = Convert.ToInt32(row["EmployeeId"]);
            var firstName = row["FirstName"].ToString();
            var lastName = row["LastName"].ToString();
            var phoneNumber = row["PhoneNumber"].ToString();
            var emailAddress = row["EmailAddress"].ToString();
            var street = row["Street"].ToString();
            var houseNumber = row["HouseNumber"].ToString();
            var zipCode = row["ZipCode"].ToString();
            var city = row["City"].ToString();

            //var teamMembers = GetTeamMembers(id).ToList();  // TODO Won't work, infinite loop, get teammembers > get teamleader > get teamleaders > ect
            var teamMembers = new List<ProductionWorker>();

            return new TeamLeader(
                id: id,
                firstName: firstName,
                lastName: lastName,
                phoneNumber: phoneNumber,
                emailAddress: emailAddress,
                street: street,
                houseNumber: houseNumber,
                zipCode: zipCode,
                city: city,
                teamMembers: teamMembers
                );
        }

        public static IEnumerable<SqlParameter> GetSqlParametersFromTeamLeader(TeamLeader teamLeader, bool withId)
        {
            return GetEmployeeParameters(teamLeader, withId);
        }

        public static SalesManager GetSalesManagerFromDataRow(DataRow row)
        {
            var id = Convert.ToInt32(row["EmployeeId"]);
            var firstName = row["FirstName"].ToString();
            var lastName = row["LastName"].ToString();
            var phoneNumber = row["PhoneNumber"].ToString();
            var emailAddress = row["EmailAddress"].ToString();
            var street = row["Street"].ToString();
            var houseNumber = row["HouseNumber"].ToString();
            var zipCode = row["ZipCode"].ToString();
            var city = row["City"].ToString();

            return new SalesManager(
                id: id,
                firstName: firstName,
                lastName: lastName,
                phoneNumber: phoneNumber,
                emailAddress: emailAddress,
                street: street,
                houseNumber: houseNumber,
                zipCode: zipCode,
                city: city
                );
        }

        public static IEnumerable<SqlParameter> GetSqlParametersFromSalesManager(SalesManager hrManager, bool withId)
        {
            return GetEmployeeParameters(hrManager, withId);
        }

        public static ProductionWorker GetProductionWorkerFromDataRow(DataRow row)
        {
            var id = Convert.ToInt32(row["EmployeeId"]);
            var firstName = row["FirstName"].ToString();
            var lastName = row["LastName"].ToString();
            var phoneNumber = row["PhoneNumber"].ToString();
            var emailAddress = row["EmailAddress"].ToString();
            var street = row["Street"].ToString();
            var houseNumber = row["HouseNumber"].ToString();
            var zipCode = row["ZipCode"].ToString();
            var city = row["City"].ToString();
            
            TeamLeader teamLeader = null;
            if (row["TeamLeaderId"] != DBNull.Value)
            {
                var teamLeaderId = Convert.ToInt32(row["TeamLeaderId"]);
                teamLeader = new EmployeeRepo(ContextType.Mssql).GetTeamLeaderById(teamLeaderId); // TODO Dit kan netter, geen repo aanroepen
                //teamLeader = new TeamLeader(1,"sample","sample");
            }
            
            return new ProductionWorker(
                id: id,
                firstName: firstName,
                lastName: lastName,
                phoneNumber: phoneNumber,
                emailAddress: emailAddress,
                street: street,
                houseNumber: houseNumber,
                zipCode: zipCode,
                city: city,
                teamLeader: teamLeader
                );
        }

        public static IEnumerable<SqlParameter> GetSqlParametersFromProductionWorker(ProductionWorker productionWorker, bool withId)
        {
            //var parameters = new List<SqlParameter>()
            //{
            //    new SqlParameter("@FirstName", productionWorker.FirstName),
            //    new SqlParameter("@LastName", productionWorker.LastName),
            //    new SqlParameter("@PhoneNumber", productionWorker.PhoneNumber),
            //    new SqlParameter("@EmailAddress", productionWorker.EmailAddress),
            //    new SqlParameter("@Street", productionWorker.Street),
            //    new SqlParameter("@HouseNumber", productionWorker.HouseNumber),
            //    new SqlParameter("@ZipCode", productionWorker.ZipCode),
            //    new SqlParameter("@City", productionWorker.City)
            //};
            //if (productionWorker.TeamLeader == null)
            //{
            //    parameters.Add(new SqlParameter("@TeamLeaderId", DBNull.Value));
            //}
            //else
            //{
            //    parameters.Add(new SqlParameter("@TeamLeaderId", productionWorker.TeamLeader.Id));
            //}
            //if (withId)
            //{
            //    parameters.Add(new SqlParameter("@Id", productionWorker.Id));
            //}
            //foreach (var parameter in parameters)
            //{
            //    if (parameter.Value == null) parameter.Value = DBNull.Value;
            //}
            //return parameters;
            var parameters = GetEmployeeParameters(productionWorker, withId);
            parameters.Add(new SqlParameter("@TeamLeaderId", productionWorker.TeamLeader.Id));
            return parameters;
        }

        public static HRManager GetHRManagerFromDataRow(DataRow row)
        {
            var id = Convert.ToInt32(row["EmployeeId"]);
            var firstName = row["FirstName"].ToString();
            var lastName = row["LastName"].ToString();
            var phoneNumber = row["PhoneNumber"].ToString();
            var emailAddress = row["EmailAddress"].ToString();
            var street = row["Street"].ToString();
            var houseNumber = row["HouseNumber"].ToString();
            var zipCode = row["ZipCode"].ToString();
            var city = row["City"].ToString();

            return new HRManager(
                id: id,
                firstName: firstName,
                lastName: lastName,
                phoneNumber: phoneNumber,
                emailAddress: emailAddress,
                street: street,
                houseNumber: houseNumber,
                zipCode: zipCode,
                city: city
                );
        }

        public static IEnumerable<SqlParameter> GetSqlParametersFromHRManager(HRManager hrManager, bool withId)
        {
            return GetEmployeeParameters(hrManager, withId);
        }
        #endregion
    }
}
