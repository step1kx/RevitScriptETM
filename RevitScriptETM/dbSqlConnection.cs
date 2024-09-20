using System.Data.SqlClient;


namespace RevitScriptETM
{
    internal class dbSqlConnection
    {
        public static SqlConnection conn = new SqlConnection($"Host=192.168.0.159;Port=5432;" +
            $"Username=User;" +
            $"Password=123;" +
            $"Database=Table;");

    }
}
