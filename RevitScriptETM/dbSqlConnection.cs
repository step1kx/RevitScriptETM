﻿using Npgsql;
using System.Data.SqlClient;
using System.Security.RightsManagement;


namespace RevitScriptETM
{
    public class dbSqlConnection
    {
      
        public static NpgsqlConnection connString = new NpgsqlConnection($"Host=192.168.0.159;Port=5432;" +
            $"Username=User;" +
            $"Password=123;" +
            $"Database=postgres;");

    }
}