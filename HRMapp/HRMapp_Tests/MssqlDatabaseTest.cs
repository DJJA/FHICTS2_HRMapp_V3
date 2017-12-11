using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HRMapp_Tests
{
    [TestClass]
    public class MssqlDatabaseTest
    {
        public void ValidateParameters()
        {
            // vars
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Param1", null),
                new SqlParameter("@Param2", null),
                new SqlParameter("@Param3", null)
            };

            // manipulate
            
        }
    }
}
