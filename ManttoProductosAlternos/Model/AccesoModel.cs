using System;
using System.Data.SqlClient;
using ManttoProductosAlternos.DBAccess;

namespace ManttoProductosAlternos.Model
{
    public class AccesoModel
    {
        public bool ObtenerUsuarioContraseña(string sUsuario, string sPwd)
        {
            bool bExisteUsuario = false;
            string sSql;
            SqlDataReader reader;
            SqlConnection connection;
            SqlCommand cmd;

            connection = Conexion.GetConecctionManttoCE();
            connection.Open();

            sSql = "SELECT * FROM cUsuarios WHERE usuario = @usuario AND Contrasena = @pass";
            cmd = new SqlCommand(sSql, connection);
            cmd.Parameters.AddWithValue("@usuario", sUsuario);
            cmd.Parameters.AddWithValue("@pass", sPwd);
            reader = cmd.ExecuteReader();

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

        public bool ObtenerPermisos()
        {
            bool bExisteUsuario = false;
            string sSql;
            SqlDataReader reader;
            SqlConnection connection;
            SqlCommand cmd;

            connection = Conexion.GetConecctionManttoCE();
            connection.Open();

            sSql = "SELECT * FROM cUsuarios WHERE usuario = @usuario";
            cmd = new SqlCommand(sSql, connection);
            cmd.Parameters.AddWithValue("@usuario", Environment.UserName);
            reader = cmd.ExecuteReader();

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