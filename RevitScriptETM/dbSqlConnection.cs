using System.Data.SqlClient;


namespace RevitScriptETM
{
    internal class dbSqlConnection
    {
        SqlConnection conn = new SqlConnection($@"Data Source =  Integrated Security = True", null);

    }
}
