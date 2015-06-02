using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using ManttoProductosAlternos.DBAccess;
using ManttoProductosAlternos.Dto;
using ScjnUtilities;

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

        #region Constructores

        public TemasModel()
        {
        }
        
        public TemasModel(int idProducto)
        {
            this.idProducto = idProducto;
        }


        #endregion

        public ObservableCollection<Temas> GetTemas(Temas temaPadre)
        {
            ObservableCollection<Temas> temas = new ObservableCollection<Temas>();

            SqlConnection connection = Conexion.GetConecctionManttoCE();
            
            SqlDataReader reader;
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                int idPadreBuscado = (temaPadre == null) ? 0 : temaPadre.IdTema;

                string miQry = "SELECT * FROM Temas WHERE Padre = @idPadre AND idProd = @idProducto  ORDER BY TemaStr";
                cmd = new SqlCommand(miQry, connection);
                cmd.Parameters.AddWithValue("@idPadre", idPadreBuscado);
                cmd.Parameters.AddWithValue("@idProducto", idProducto);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Temas tema = new Temas();
                    //tema.IsChecked = false;
                    tema.IdTema = Convert.ToInt32(reader["Id"]);
                    tema.Nivel = Convert.ToInt32(reader["Nivel"]);
                    tema.Padre = Convert.ToInt32(reader["Padre"]);
                    tema.Tema = reader["Tema"].ToString();
                    tema.Orden = Convert.ToInt32(reader["Orden"]);
                    tema.TemaStr = reader["TemaSTR"].ToString();
                    tema.LInicial = Convert.ToChar(reader["LetraInicial"].ToString());
                    tema.IdProducto = idProducto;

                    if (idProducto == 1)
                        tema.SubTemas = GetTemas(tema);

                    temas.Add(tema);
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                connection.Close();
            }
            return temas;
        }


        public void InsertaTemaNuevo(Temas nuevoTema)
        {
            SqlConnection connection = Conexion.GetConecctionManttoCE();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                nuevoTema.IdTema = DataBaseUtilities.GetNextIdForUse("Temas", "Id", connection);

                string sqlCadena = "SELECT * FROM Temas WHERE Id = 0";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Temas");

                dr = dataSet.Tables["Temas"].NewRow();
                dr["Id"] = nuevoTema.IdTema;
                dr["Nivel"] = nuevoTema.Nivel;
                dr["Padre"] = nuevoTema.Padre;
                dr["Tema"] = nuevoTema.Tema;
                dr["Orden"] = nuevoTema.Orden;
                dr["TemaStr"] = nuevoTema.TemaStr;
                dr["LetraInicial"] = nuevoTema.LInicial;
                dr["IdProd"] = nuevoTema.IdProducto;

                dataSet.Tables["Temas"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();

                dataAdapter.InsertCommand.CommandText = "INSERT INTO Temas (Id,Nivel,Padre,Tema,Orden,TemaStr,LetraInicial,IdProd) VALUES (@Id,@Nivel,@Padre,@Tema,@Orden,@TemaStr,@LetraInicial,@IdProd)";
                dataAdapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                dataAdapter.InsertCommand.Parameters.Add("@Nivel", SqlDbType.Int, 0, "Nivel");
                dataAdapter.InsertCommand.Parameters.Add("@Padre", SqlDbType.Int, 0, "Padre");
                dataAdapter.InsertCommand.Parameters.Add("@Tema", SqlDbType.VarChar, 0, "Tema");
                dataAdapter.InsertCommand.Parameters.Add("@Orden", SqlDbType.Int, 0, "Orden");
                dataAdapter.InsertCommand.Parameters.Add("@TemaStr", SqlDbType.VarChar, 0, "TemaStr");
                dataAdapter.InsertCommand.Parameters.Add("@LetraInicial", SqlDbType.VarChar, 0, "LetraInicial");
                dataAdapter.InsertCommand.Parameters.Add("@IdProd", SqlDbType.Int, 0, "IdProd");

                dataAdapter.Update(dataSet, "Temas");
                dataSet.Dispose();
                dataAdapter.Dispose();

                new BitacoraModel().SetBitacoraEntry(nuevoTema, 1, " ");
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                connection.Close();
            }
        }



        public void ActualizaTema(Temas tema)
        {
            SqlConnection sqlConne = Conexion.GetConecctionManttoCE();

            SqlCommand cmd;

            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            try
            {
                sqlConne.Open();

                cmd.CommandText = "UPDATE TEMAS  SET Nivel = " + tema.Nivel + ", Padre = " + tema.Padre + ",Tema = '" + tema.Tema +
                                  "',TemaStr = '" + tema.TemaStr + "' WHERE id = " + tema.IdTema + " AND idProd = " + tema.IdProducto;
                cmd.ExecuteNonQuery();
                
                

                new BitacoraModel().SetBitacoraEntry(tema, 2, " ");
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

        /// <summary>
        /// Elimina el tema seleccionado dentro del árbol 
        /// </summary>
        /// <param name="idTema"></param>
        public void EliminaTema(Temas temaEliminar)
        {
            SqlConnection sqlConne = Conexion.GetConecctionManttoCE();

            SqlCommand cmd;
            SqlDataReader dataReader;

            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            try
            {
                sqlConne.Open();

                string miQry = "select COUNT(Id) Cant FROM Temas WHERE Padre = " + temaEliminar.IdTema + " AND idProd = " + idProducto;
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

                cmd.CommandText = "DELETE FROM Temas WHERE id = " + temaEliminar.IdTema + " AND idProd = " + idProducto;
                
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM TemasIUS WHERE id = " + temaEliminar.IdTema + " AND idProd = " + idProducto;
                cmd.ExecuteNonQuery();


                new BitacoraModel().SetBitacoraEntry(temaEliminar, 3, String.Empty);
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("El tema que desea eliminar contiene subtemas, elimine primero los subtemas para completar la operación", "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlConne.Close();
            }
        }

        public ObservableCollection<Temas> GetTemasRelacionados(long ius)
        {
            ObservableCollection<Temas> temas = new ObservableCollection<Temas>();
            SqlConnection connection = Conexion.GetConecctionManttoCE();

            SqlDataReader reader;
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                string miQry = "SELECT T.Id,T.Tema " +
                               " FROM Temas T INNER JOIN TemasIUS I ON I.Id = T.Id  " +
                               " WHERE (I.IUS = @ius AND T.IdProd = @idProducto ) AND I.idProd = @idProducto " +
                               " ORDER BY T.TemaStr";

                cmd = new SqlCommand(miQry, connection);
                cmd.Parameters.AddWithValue("@ius", ius);
                cmd.Parameters.AddWithValue("@idProducto", idProducto);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Temas tema = new Temas();
                    tema.IdTema = Convert.ToInt32(reader["Id"]) ;
                    tema.Tema = reader["Tema"].ToString();

                    temas.Add(tema);
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                connection.Close();
            }

            return temas;
        }

        public void EliminaRelacion(long ius)
        {
            SqlConnection connection = Conexion.GetConecctionManttoCE();
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM TemasIUS WHERE IUS = " + ius + " AND idProd = " + idProducto;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Tesis WHERE IUS = " + ius + " AND idProd = " + idProducto;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "insert into Bitacora(idTema,tipoModif,edoAnterior,usuario,idProd)" +
                                  "values(" + ius + ",12,'0-" + ius + " ','" + Environment.MachineName + "'," + idProducto + ")";
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
                connection.Close();
            }
        }

        /// <summary>
        /// Obtiene el listado de temas completo de acuerdo a la materia seleccionada
        /// para la posterior verificación en busca de temas duplicados
        /// </summary>
        /// <param name="idProducto"></param>
        /// <returns></returns>
        public List<Temas> GetTemasForReview(int idProducto)
        {
            List<Temas> temas = new List<Temas>();
            SqlConnection sqlNueva = Conexion.GetConecctionManttoCE();

            SqlDataReader dataReader;
            SqlCommand cmdAntes;

            cmdAntes = sqlNueva.CreateCommand();
            cmdAntes.Connection = sqlNueva;

            try
            {
                sqlNueva.Open();

                string miQry = "SELECT TemaStr  FROM Temas WHERE idProd = @IdProducto ORDER BY TemaStr";

                cmdAntes = new SqlCommand(miQry, sqlNueva);
                cmdAntes.Parameters.AddWithValue("@IdProducto", idProducto);
                dataReader = cmdAntes.ExecuteReader();

                while (dataReader.Read())
                {
                    Temas tema = new Temas();
                    tema.TemaStr = dataReader["TemaStr"].ToString();

                    temas.Add(tema);
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

            return temas;
        }

        public void SearchForDuplicates(ObservableCollection<List<Temas>> repetidos, string temaBuscado, int idProducto)
        {
            List<Temas> temas = new List<Temas>();
            SqlConnection connection = Conexion.GetConecctionManttoCE();

            SqlDataReader reader;
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                string miQry = "SELECT Id,TemaStr  FROM Temas WHERE TemaStr = @temaBuscado and IDProd = @idProducto";

                cmd = new SqlCommand(miQry, connection);
                cmd.Parameters.AddWithValue("@temaBuscado", temaBuscado);
                cmd.Parameters.AddWithValue("@idProducto", idProducto);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Temas tema = new Temas();
                    tema.IdTema = reader["Id"] as Int16? ?? -1;
                    tema.Tema = reader["TemaStr"].ToString().Trim();

                    temas.Add(tema);
                }
                reader.Close();

                if (temas.Count >= 2)
                    repetidos.Add(temas);
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}