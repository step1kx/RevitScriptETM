using Npgsql;
using System;
using System.Data.SqlClient;
using System.Security.RightsManagement;
using System.Windows;


namespace RevitScriptETM
{
    public class dbSqlConnection
    {
        public static string connString = "Server=192.168.0.171; Port=5432 ; User Id = User ; Password = 123; Database = postgres";
    }
}
