using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ManttoProductosAlternos.DBAccess
{
    public class Conexion
    {
        public static SqlConnection GetConnectMantesis()
        {
            String bd = ConfigurationManager.ConnectionStrings["BaseMantesis"].ConnectionString;

            SqlConnection realConnection = new SqlConnection(bd);
            return realConnection;
        }

        public static SqlConnection GetConecctionManttoCE()
        {
            String bdStringSql = ConfigurationManager.ConnectionStrings["BaseMantenimiento"].ConnectionString;

            SqlConnection realConnection = new SqlConnection(bdStringSql);
            return realConnection;
        }

        public static SqlConnection GetConnectionCt9bd3()
        {
            String bdStringSql = ConfigurationManager.ConnectionStrings["BaseIUS"].ConnectionString;

            SqlConnection realConnection = new SqlConnection(bdStringSql);
            return realConnection;
        }
    }
}