using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using UtilsAlternos;

namespace ManttoProductosAlternos.DBAccess
{
    public class Conexion
    {
        public static DbConnection GetConnectMant()
        {

            String BD = ConfigurationManager.ConnectionStrings["BaseMantesis"].ConnectionString;
                //ConnectionStringSettings settings = new ConnectionStringSettings("Bd", "Data Source=" + BD + "; Persist Security Info=False;Provider=Microsoft.Jet.OLEDB.4.0; Mode=ReadWrite|Share Deny None");
                ConnectionStringSettings settings = new ConnectionStringSettings("Bd", "Data Source=" + BD + "; Provider=Microsoft.Jet.OLEDB.4.0; Mode=ReadWrite|Share Deny None");

                //string connectionString = settings.ConnectionString;
                DbConnection realConnection = new OleDbConnection(BD);
                return realConnection;
            
        }

        public static DbConnection GetConecctionManttoCE()
        {
            String bdStringSql = ConfigurationManager.ConnectionStrings["BaseMantenimiento"].ConnectionString;
            //bdStringSql = "Data Source=CT9BD2;Initial Catalog=ManttoCE;User Id=manttoce;Password=manttoce2012";
            //bdStringSql = "Data Source=CT9BD2;Initial Catalog=ManttoCEDesa;User Id=manttoce;Password=manttoce2012";
            DbConnection realConnection = new SqlConnection(bdStringSql);
            return realConnection;
        }

        public static DbConnection GetConecctionDsql()
        {
            String bdStringSql = ConfigurationManager.ConnectionStrings["BaseIUS"].ConnectionString;
            //bdStringSql = "Data Source=CGCSTDSQL;Initial Catalog=ius;User Id=4cc3s01nf0;Password=Pr0gr4m4d0r3s";
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
