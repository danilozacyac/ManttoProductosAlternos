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
            SqlConnection connection = Conexion.GetConecctionManttoCE();

            SqlDataReader dataReader;
            SqlCommand cmdAntes;

            cmdAntes = connection.CreateCommand();
            cmdAntes.Connection = connection;

            try
            {
                connection.Open();

                string miQry = "SELECT * FROM cUsuarios";

                cmdAntes = new SqlCommand(miQry, connection);
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
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UsuariosModel", "ManttoProductosAlternos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UsuariosModel", "ManttoProductosAlternos");
            }
            finally
            {
                connection.Close();
            }

            return usuarios;
        }

        public void UpdateUsuario(Usuarios usuario)
        {
            SqlConnection connection = Conexion.GetConecctionManttoCE();
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

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
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UsuariosModel", "ManttoProductosAlternos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UsuariosModel", "ManttoProductosAlternos");
            }
            finally
            {
                connection.Close();
            }
        }

        public void SetNewUsuario(Usuarios usuario)
        {
            SqlConnection connection = Conexion.GetConecctionManttoCE();
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                String query = "INSERT INTO cUsuarios VALUES((SELECT (MAX(Llave) + 1) FROM cUsuarios),'" + usuario.Nombre + "','" + usuario.Usuario + "','" + usuario.Contrasena + "','" + usuario.Autorizados + "'," + usuario.Grupo + ")";

                cmd.CommandText = query;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Usuario agregado correctamente");
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UsuariosModel", "ManttoProductosAlternos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UsuariosModel", "ManttoProductosAlternos");
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
