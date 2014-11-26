using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace ManttoProductosAlternos.DBAccess
{
    public class Conexion
    {
        public static SqlConnection GetConnectMant()
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

        public static SqlConnection GetConecctionDsql()
        {
            String bdStringSql = ConfigurationManager.ConnectionStrings["BaseIUS"].ConnectionString;

            SqlConnection realConnection = new SqlConnection(bdStringSql);
            return realConnection;
        }
    }
}