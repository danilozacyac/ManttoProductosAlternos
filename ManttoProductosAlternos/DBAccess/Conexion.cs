using System;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Windows;

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
            String bdStringSql;

            string tipoApp = ConfigurationManager.AppSettings["TipoAplicacion"].ToString();

            if (tipoApp.Equals("NPRUEBA"))
                bdStringSql = ConfigurationManager.ConnectionStrings["BaseMantenimiento"].ConnectionString;
            else
            {
                bdStringSql = ConfigurationManager.ConnectionStrings["BasePrueba"].ConnectionString;
                //MessageBox.Show("Estas viendo datos de prueba, comunicate con tu administrador");
            }

            SqlConnection realConnection = new SqlConnection(bdStringSql);
            return realConnection;
        }

        public static string GetConecctionStringManttoCE()
        {
            String bdStringSql;

            string tipoApp = ConfigurationManager.AppSettings["TipoAplicacion"].ToString();

            if (tipoApp.Equals("NPRUEBA"))
                bdStringSql = ConfigurationManager.ConnectionStrings["BaseMantenimiento"].ConnectionString;
            else
            {
                bdStringSql = ConfigurationManager.ConnectionStrings["BasePrueba"].ConnectionString;
                MessageBox.Show("Estas viendo datos de prueba, comunicate con tu administrador");
            }

            return bdStringSql;
        }

        public static SqlConnection GetConnectionCt9bd3()
        {
            String bdStringSql = ConfigurationManager.ConnectionStrings["BaseIUS"].ConnectionString;

            SqlConnection realConnection = new SqlConnection(bdStringSql);
            return realConnection;
        }


        public static OleDbConnection GetAccessDataBaseConnection(int idProducto)
        {
            String bdStringAccess = "";

            switch (idProducto)
            {
                case 2:
                    bdStringAccess = ConfigurationManager.ConnectionStrings["Suspension"].ToString();
                    break;
                case 3:
                    bdStringAccess = ConfigurationManager.ConnectionStrings["Improcedencia"].ToString();
                    break;
                case 4:
                    bdStringAccess = ConfigurationManager.ConnectionStrings["Facultades"].ToString();
                    break;
                case 15:
                    bdStringAccess = ConfigurationManager.ConnectionStrings["Electoral"].ToString();
                    break;

            }
            OleDbConnection accessConnection = new OleDbConnection(bdStringAccess);

            return accessConnection;
        }
    }
}