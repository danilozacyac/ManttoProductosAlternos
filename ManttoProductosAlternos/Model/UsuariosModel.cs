using System;
using System.Collections.Generic;
using ManttoProductosAlternos.Dto;
using System.Data.SqlClient;
using ManttoProductosAlternos.DBAccess;
using ScjnUtilities;
using System.Windows.Forms;

namespace ManttoProductosAlternos.Model
{
    public class UsuariosModel
    {
        public List<Usuarios> GetUsuarios()
        {
            List<Usuarios> usuarios = new List<Usuarios>();
            SqlConnection sqlNueva = Conexion.GetConecctionManttoCE();

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
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                sqlNueva.Close();
            }

            return usuarios;
        }

        public void UpdateUsuario(Usuarios usuario)
        {
            SqlConnection sqlConne = Conexion.GetConecctionManttoCE();
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
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                sqlConne.Close();
            }
        }

        public void SetNewUsuario(Usuarios usuario)
        {
            SqlConnection sqlConne = Conexion.GetConecctionManttoCE();
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
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                sqlConne.Close();
            }
        }
    }
}
