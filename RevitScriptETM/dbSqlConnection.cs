using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitScriptETM
{
    internal class dbSqlConnection
    {
        SqlConnection conn = new SqlConnection($@"Data Source =  Integrated Security = True", null);

    }
}
