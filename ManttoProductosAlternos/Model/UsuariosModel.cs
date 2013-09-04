using System;
using System.Collections.Generic;
using ManttoProductosAlternos.DTO;
using System.Windows;
using System.Data.SqlClient;
using ManttoProductosAlternos.DBAccess;

namespace ManttoProductosAlternos.Model
{
    public class UsuariosModel
    {
        public List<Usuarios> GetUsuarios()
        {
            List<Usuarios> usuarios = new List<Usuarios>();
            SqlConnection sqlNueva = (SqlConnection)Conexion.GetConecctionManttoCE();

            SqlDataReader dataReader;
            SqlCommand cmdAntes;

            cmdAntes = sqlNueva.CreateCommand();
            cmdAntes.Connection = sqlNueva;

            try
            {
                sqlNueva.Open();

                string miQry = "SELECT * FROM cUsuarios";

                cmdAntes = new SqlCommand(miQry, sqlNueva);
                dataReader = cmdAntes.ExecuteReader();

                while (dataReader.Read())
                {
                    Usuarios usuario = new Usuarios();
                    usuario.Llave = Convert.ToInt32(dataReader["Llave"]);
                    usuario.Nombre = dataReader["Nombre"].ToString();
                    usuario.Usuario = dataReader["Usuario"].ToString();
                    usuario.Contrasena = dataReader["Contrasena"].ToString();
                    usuario.Autorizados = dataReader["ProgAutorizados"].ToString();
                    usuario.Grupo = Convert.ToInt16(dataReader["Grupo"]);
                    usuarios.Add(usuario);
                }
                dataReader.Close();
            }
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            finally
            {
                sqlNueva.Close();
            }

            return usuarios;
        }

        public void UpdateUsuario(Usuarios usuario)
        {
            SqlConnection sqlConne = (SqlConnection)Conexion.GetConecctionManttoCE();
            SqlCommand cmd;

            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            try
            {
                sqlConne.Open();

                String query = "UPDATE cUsuarios SET Contrasena = '" + usuario.Contrasena + "'";

                if (usuario.Grupo > 0)
                {
                    query += ", Grupo = " + usuario.Grupo + ", ProgAutorizados = '" + usuario.Autorizados + "'";
                }

                query += " WHERE Llave = " + usuario.Llave;

                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }
        }

        public void SetNewUsuario(Usuarios usuario)
        {
            SqlConnection sqlConne = (SqlConnection)Conexion.GetConecctionManttoCE();
            SqlCommand cmd;

            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            try
            {
                sqlConne.Open();

                String query = "INSERT INTO cUsuarios VALUES((SELECT (MAX(Llave) + 1) FROM cUsuarios),'" + usuario.Nombre + "','" + usuario.Usuario + "','" + usuario.Contrasena + "','" + usuario.Autorizados + "'," + usuario.Grupo + ")";

                cmd.CommandText = query;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Usuario agregado correctamente");
            }
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }
        }
    }
}
