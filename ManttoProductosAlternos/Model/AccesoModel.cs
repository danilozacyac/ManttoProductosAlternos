using System;
using System.Data;
using System.Data.Common;
using System.Windows;
using ManttoProductosAlternos.DBAccess;

namespace ManttoProductosAlternos.Model
{
    public class AccesoModel
    {

        private DataTableReader GetDatosTabla(string sqlString, DbConnection lConn)
        {
            DataTableReader dtr = null;
            try
            {
                //lConn.Open();
                DataAdapter query = Conexion.GetDataAdapter(sqlString, lConn);
                dtr = Conexion.GetDatosReader(query);
            }
            catch (DbException ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            catch (SystemException sex)
            {
                MessageBox.Show("Error ({0}) : {1}" + sex.Source + sex.Message, "Error Interno");
            }
            return dtr;
        }

        public bool ObtenerUsuarioContraseña(string sUsuario, string sPwd)
        {
            bool bExisteUsuario = false;
            string sSql;
            DataTableReader reader;
            DbConnection connection;

            connection = Conexion.GetConecctionManttoCE();
            connection.Open();

            sSql = "SELECT * FROM cUsuarios WHERE usuario = '" + sUsuario + "' AND Contrasena ='" + sPwd + "'";
            reader = this.GetDatosTabla(sSql, connection);

            if (reader.Read())
            {
                AccesoUsuarioModel.Usuario = reader["usuario"].ToString();
                AccesoUsuarioModel.Pwd = reader["contrasena"].ToString();
                AccesoUsuarioModel.Llave = Convert.ToInt16(reader["Llave"].ToString());
                AccesoUsuarioModel.Grupo = Convert.ToInt16(reader["Grupo"].ToString());
                AccesoUsuarioModel.Programas = reader["ProgAutorizados"].ToString();
                AccesoUsuarioModel.Nombre = reader["nombre"].ToString();

                bExisteUsuario = true;
            }
            else
            {
                AccesoUsuarioModel.Llave = -1;
            }

            return bExisteUsuario;
        }

    }
}
