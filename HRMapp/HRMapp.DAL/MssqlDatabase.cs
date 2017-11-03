using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.DAL
{
    public abstract class MssqlDatabase
    {
        protected readonly string connectionString = "Server=tcp:djjaserver.database.windows.net,1433;Initial Catalog=HRMapp;Persist Security Info=False;User ID=djja;Password=DrEh437u;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }
}
