using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using ManttoProductosAlternos.DBAccess;
using ManttoProductosAlternos.DTO;
using ManttoProductosAlternos.Utils;

namespace ManttoProductosAlternos.Model
{
    public class TemasModel
    {
        /*
        En la bitacora de la base de datos el tipo de Modificación tiene la siguiente nomenclatura
        * 1. Tema Nuevo
        * 2. Actualización de Tema
        * 3. Elimina Tema
        * 11. Agrega Relación
        * 12. Elimina Relación
        */
        private readonly int idProducto;

        public TemasModel(int idProducto)
        {
            this.idProducto = idProducto;
        }

        //public List<Temas> GetTemas(int idPadre)
        //{
        //    List<Temas> temas = new List<Temas>();

        //    SqlConnection sqlConne = (SqlConnection)Conexion.GetConecctionManttoCE();
        //    SqlDataReader dataReader;
        //    SqlCommand cmd;

        //    cmd = sqlConne.CreateCommand();
        //    cmd.Connection = sqlConne;

        //    try
        //    {
        //        sqlConne.Open();

        //        string miQry = "select * from Temas Where Padre = " + idPadre + " AND idProd = " + idProducto + "  ORDER BY TemaStr";
        //        cmd = new SqlCommand(miQry, sqlConne);
        //        dataReader = cmd.ExecuteReader();

        //        while (dataReader.Read())
        //        {
        //            Temas tema = new Temas();
        //            tema.IsChecked = false;
        //            tema.Id = Convert.ToInt32(dataReader["Id"].ToString());
        //            tema.Nivel = Convert.ToInt32(dataReader["Nivel"].ToString()); 
        //            tema.Padre = Convert.ToInt32(dataReader["Padre"].ToString()); 
        //            tema.Tema = dataReader["Tema"].ToString();
        //            tema.Orden = Convert.ToInt32(dataReader["Orden"].ToString());
        //            tema.TemaStr = dataReader["TemaSTR"].ToString();
        //            tema.LInicial = Convert.ToChar(dataReader["LetraInicial"].ToString());

        //            temas.Add(tema);
        //        }
        //        dataReader.Close();
        //        temas = temas.Distinct().ToList();
        //    }
        //    catch (SqlException sql)
        //    {
        //        MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
        //    }
        //    finally
        //    {
        //        sqlConne.Close();
        //    }
        //    return temas;
        //}

        public ObservableCollection<Temas> GetTemas(int idPadre)
        {
            ObservableCollection<Temas> temas = new ObservableCollection<Temas>();

            SqlConnection sqlConne = (SqlConnection)Conexion.GetConecctionManttoCE();
            SqlDataReader dataReader;
            SqlCommand cmd;

            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            try
            {
                sqlConne.Open();

                string miQry = "select * from Temas Where Padre = " + idPadre + " AND idProd = " + idProducto + "  ORDER BY TemaStr";
                cmd = new SqlCommand(miQry, sqlConne);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Temas tema = new Temas();
                    tema.IsChecked = false;
                    tema.Id = Convert.ToInt32(dataReader["Id"].ToString());
                    tema.Nivel = Convert.ToInt32(dataReader["Nivel"].ToString());
                    tema.Padre = Convert.ToInt32(dataReader["Padre"].ToString());
                    tema.Tema = dataReader["Tema"].ToString();
                    tema.Orden = Convert.ToInt32(dataReader["Orden"].ToString());
                    tema.TemaStr = dataReader["TemaSTR"].ToString();
                    tema.LInicial = Convert.ToChar(dataReader["LetraInicial"].ToString());

                    temas.Add(tema);
                }
                dataReader.Close();
                //temas = temas.Distinct().ToList();
            }
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }
            return temas;
        }

        public void InsertaTemaNuevo(Temas tema)
        {
            SqlConnection sqlConne = (SqlConnection)Conexion.GetConecctionManttoCE();

            SqlCommand cmd;
            SqlDataReader dataReader;

            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            int idSiguiente = 0;

            try
            {
                sqlConne.Open();

                string miQry = "SELECT MAX(id) Id FROM Temas";
                cmd = new SqlCommand(miQry, sqlConne);
                dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    idSiguiente = (Convert.ToInt32(dataReader["id"].ToString())) + 1;

                    dataReader.Close();

                    cmd.CommandText = "INSERT INTO TEMAS VALUES (" + idSiguiente + "," + tema.Nivel + "," + tema.Padre + ",'" + tema.Tema +
                                      "'," + tema.Orden + ",'" + tema.TemaStr + "','" + tema.LInicial + "'," + tema.IdProd + ")";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "insert into Bitacora(idTema,tipoModif,edoAnterior,usuario,idProd)" +
                                      "values(" + idSiguiente + ",1,' ','" + Environment.MachineName + "'," + tema.IdProd + ")";
                    cmd.ExecuteNonQuery();

                    VarGlobales.idSiguiente = idSiguiente;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Error ({0}) : {1} No se pudo asignar un identificador al tema, intentar más tarde", "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }
        }

        public void ActualizaTema(Temas tema)
        {
            SqlConnection sqlConne = (SqlConnection)Conexion.GetConecctionManttoCE();

            SqlCommand cmd;

            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            try
            {
                sqlConne.Open();

                cmd.CommandText = "UPDATE TEMAS  SET Nivel = " + tema.Nivel + ", Padre = " + tema.Padre + ",Tema = '" + tema.Tema +
                                  "',TemaStr = '" + tema.TemaStr + "' WHERE id = " + tema.Id + " AND idProd = " + tema.IdProd;
                cmd.ExecuteNonQuery();
                
                cmd.CommandText = "insert into Bitacora(idTema,tipoModif,edoAnterior,usuario,idProd)" +
                                  "values(" + tema.Id + ",2,' ','" + Environment.MachineName + "'," + tema.IdProd + ")";
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

        /// <summary>
        /// Elimina el tema seleccionado dentro del árbol 
        /// </summary>
        /// <param name="idTema"></param>
        public void EliminaTema(int idTema)
        {
            SqlConnection sqlConne = (SqlConnection)Conexion.GetConecctionManttoCE();

            SqlCommand cmd;
            SqlDataReader dataReader;

            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            try
            {
                sqlConne.Open();

                string miQry = "select COUNT(Id) Cant FROM Temas WHERE Padre = " + idTema + " AND idProd = " + idProducto;
                cmd = new SqlCommand(miQry, sqlConne);
                dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    if (Convert.ToInt32(dataReader["Cant"].ToString()) > 0)
                        throw new OperationCanceledException();
                }
                else
                    throw new OperationCanceledException();

                dataReader.Close();

                cmd.CommandText = "DELETE FROM Temas WHERE id = " + idTema + " AND idProd = " + idProducto;
                
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM TemasIUS WHERE id = " + idTema + " AND idProd = " + idProducto;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "insert into Bitacora(idTema,tipoModif,edoAnterior,usuario,idProd)" +
                                  "values(" + idTema + ",3,' ','" + Environment.MachineName + "'," + idProducto + ")";
                cmd.ExecuteNonQuery();
            }
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("El tema que desea eliminar contiene subtemas, elimine primero los subtemas para completar la operación", "Error Interno", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlConne.Close();
            }
        }

        public List<Temas> GetTemasRelacionados(long ius)
        {
            List<Temas> temas = new List<Temas>();
            SqlConnection sqlNueva = (SqlConnection)Conexion.GetConecctionManttoCE();

            SqlDataReader dataReader;
            SqlCommand cmdAntes;

            cmdAntes = sqlNueva.CreateCommand();
            cmdAntes.Connection = sqlNueva;

            try
            {
                sqlNueva.Open();

                string miQry = "SELECT T.Id,T.Tema " +
                               " FROM Temas T INNER JOIN TemasIUS I ON I.Id = T.Id  " +
                               " WHERE (I.IUS = " + ius + " AND T.IdProd = " + idProducto + " ) AND I.idProd = " + idProducto + " " +
                               " ORDER BY T.TemaStr";

                cmdAntes = new SqlCommand(miQry, sqlNueva);
                dataReader = cmdAntes.ExecuteReader();

                while (dataReader.Read())
                {
                    Temas tema = new Temas();
                    tema.Id = Convert.ToInt32(dataReader["Id"].ToString());
                    tema.Tema = dataReader["Tema"].ToString();

                    temas.Add(tema);
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

            return temas;
        }

        public void EliminaRelacion(long ius)
        {
            SqlConnection b2Conne = (SqlConnection)Conexion.GetConecctionManttoCE();
            SqlCommand cmd;

            cmd = b2Conne.CreateCommand();
            cmd.Connection = b2Conne;

            try
            {
                b2Conne.Open();

                cmd.CommandText = "DELETE FROM TemasIUS WHERE IUS = " + ius + " AND idProd = " + idProducto;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Tesis WHERE IUS = " + ius + " AND idProd = " + idProducto;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "insert into Bitacora(idTema,tipoModif,edoAnterior,usuario,idProd)" +
                                  "values(" + ius + ",12,'0-" + ius + " ','" + Environment.MachineName + "'," + idProducto + ")";
                cmd.ExecuteNonQuery();
            }
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            finally
            {
                b2Conne.Close();
            }
        }
    }
}