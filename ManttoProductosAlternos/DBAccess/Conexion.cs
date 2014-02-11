using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace ManttoProductosAlternos.DBAccess
{
    public class Conexion
    {
        public static DbConnection GetConnectMant()
        {
            String bd = ConfigurationManager.ConnectionStrings["BaseMantesis"].ConnectionString;
                
            DbConnection realConnection = new OleDbConnection(bd);
            return realConnection;
        }

        public static DbConnection GetConecctionManttoCE()
        {
            String bdStringSql = ConfigurationManager.ConnectionStrings["BaseMantenimiento"].ConnectionString;

            DbConnection realConnection = new SqlConnection(bdStringSql);
            return realConnection;
        }
        
        public static DbConnection GetConecctionDsql()
        {
            String bdStringSql = ConfigurationManager.ConnectionStrings["BaseIUS"].ConnectionString;
            
            DbConnection realConnection = new SqlConnection(bdStringSql);
            return realConnection;
        }

        public static DataAdapter GetDataAdapter(String sql, DbConnection conexion)
        {
            if (conexion.GetType() == typeof(SqlConnection))
            {
                SqlConnection sqlDA = (SqlConnection)conexion;
                return new SqlDataAdapter(sql, sqlDA);
            }
            else
            {
                OleDbConnection oleDA = (OleDbConnection)conexion;
                return new OleDbDataAdapter(sql, oleDA);
            }
        }

        public static DataTableReader GetDatosReader(DataAdapter query)
        {
            DataSet dataSet = new DataSet();
            query.Fill(dataSet);
            DataTableReader reader = dataSet.Tables[0].CreateDataReader();

            return reader;
        }
    }
}