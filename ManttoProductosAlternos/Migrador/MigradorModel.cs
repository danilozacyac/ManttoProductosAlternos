using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using ManttoProductosAlternos.DBAccess;
using ManttoProductosAlternos.Dto;
using ScjnUtilities;

namespace ManttoProductosAlternos.Migrador
{
    public class MigradorModel
    {
        private readonly int idProducto;

        public MigradorModel(int idProducto)
        {
            this.idProducto = idProducto;
        }

        /// <summary>
        /// Elimina la información previa contenida en las bases de datos de access donde se deposita
        /// la información.
        /// </summary>
        public void EliminaRegistros()
        {
            OleDbConnection connection = Conexion.GetAccessDataBaseConnection(idProducto);
            OleDbCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                if (idProducto != 4)
                {

                    cmd.CommandText = "DELETE FROM IUS";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM Temas";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM TemasIUS";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = "DELETE FROM Tesis";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (OleDbException ex)
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

        #region Números de IUS

        /// <summary>
        /// Obtiene el número de IUS de cada una de las tesis relacionadas con alguno de los temas de 
        /// el producto en específico y los envía al Método insertaTemasIus
        /// </summary>
        /// <returns>Devuelve el número de tesis relacionadas</returns>
        public int GetTesisRelacionadasByProducto()
        {
            List<int> numerosDeIus = new List<int>();

            SqlConnection connection = Conexion.GetConecctionManttoCE();

            SqlDataReader dataReader;
            SqlCommand cmd;

            try
            {
                connection.Open();

                string miQry = "SELECT IUS FROM Tesis WHERE IdProd = @IdProd ORDER BY ConsecIndx";
                cmd = new SqlCommand(miQry, connection);
                cmd.Parameters.AddWithValue("@IdProd", idProducto);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    numerosDeIus.Add(Convert.ToInt32(dataReader["IUS"]));
                }
                dataReader.Close();
                this.InsertaIuses(numerosDeIus);

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


            return numerosDeIus.Count;
        }

        /// <summary>
        /// Recibe los números de IUS relacionados con cada uno de los productos y los ingresa en la tabla
        /// de nombre IUS
        /// </summary>
        /// <param name="numerosDeIus"></param>
        private void InsertaIuses(List<int> numerosDeIus)
        {
            OleDbConnection connection = Conexion.GetAccessDataBaseConnection(idProducto);
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            string sqlCadena = "SELECT * FROM IUS WHERE IUS = 0";

            try
            {
                connection.Open();

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Tesis");

                foreach (int ius in numerosDeIus)
                {
                    dr = dataSet.Tables["Tesis"].NewRow();
                    dr["IUS"] = ius;

                    dataSet.Tables["Tesis"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connection.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO IUS(IUS) VALUES(@IUS)";
                    dataAdapter.InsertCommand.Parameters.Add("@IUS", OleDbType.Numeric, 0, "IUS");

                    dataAdapter.Update(dataSet, "Tesis");

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                }
            }
            catch (OleDbException ex)
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

        #endregion

        #region Facultades de la Suprema Corte

        public int GetTesisRelacionadasScjn()
        {
            List<TesisDTO> listaTesis = new List<TesisDTO>();
            SqlConnection connection = Conexion.GetConecctionManttoCE();

            SqlDataReader reader;
            SqlCommand cmdAntes;

            cmdAntes = connection.CreateCommand();
            cmdAntes.Connection = connection;

            try
            {
                connection.Open();

                string miQry = "SELECT *  FROM TemasIUS WHERE IdProd = " + idProducto +
                                " ORDER BY id,ConsecIndx";
                cmdAntes = new SqlCommand(miQry, connection);
                reader = cmdAntes.ExecuteReader();

                while (reader.Read())
                {
                    TesisDTO tesis = new TesisDTO();
                    tesis.Ius = Convert.ToInt32(reader["IUS"].ToString());
                    tesis.Parte = Convert.ToInt32(reader["Id"].ToString());
                    tesis.Volumen = Convert.ToInt32(reader["ConsecIndx"].ToString());

                    listaTesis.Add(tesis);
                }
                reader.Close();
                this.InsertaTemasIusScjn(listaTesis);

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


            return listaTesis.Count;
        }

        private void InsertaTemasIusScjn(List<TesisDTO> listaTesis)
        {
            OleDbConnection connection = Conexion.GetAccessDataBaseConnection(idProducto);
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            string sqlCadena = "SELECT * FROM Tesis WHERE IUS4 = 0";

            try
            {
                connection.Open();

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Tesis");

                int consec = 1;

                foreach (TesisDTO tesis in listaTesis)
                {
                    dr = dataSet.Tables["Tesis"].NewRow();
                    dr["Tipo"] = tesis.Parte;
                    dr["Consec"] = consec;
                    dr["IUS4"] = tesis.Ius;
                    dr["ConsecIndx"] = tesis.Volumen;

                    dataSet.Tables["Tesis"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connection.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO Tesis(TIPO,CONSEC,IUS4,CONSECINDX) VALUES(@TIPO,@CONSEC,@IUS4,@CONSECINDX)";
                    dataAdapter.InsertCommand.Parameters.Add("@TIPO", OleDbType.Numeric, 0, "TIPO");
                    dataAdapter.InsertCommand.Parameters.Add("@CONSEC", OleDbType.Numeric, 0, "CONSEC");
                    dataAdapter.InsertCommand.Parameters.Add("@IUS4", OleDbType.Numeric, 0, "IUS4");
                    dataAdapter.InsertCommand.Parameters.Add("@CONSECINDX", OleDbType.Numeric, 0, "CONSECINDX");

                    dataAdapter.Update(dataSet, "Tesis");

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                    consec++;
                }
            }
            catch (OleDbException ex)
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

        #endregion

        #region Temas


        /// <summary>
        /// Obtiene los temas de el producto que se esta migrando y los envia al método de insertar temas
        /// </summary>
        /// <returns></returns>
        public int GetTemas()
        {
            List<Temas> temas = new List<Temas>();

            SqlConnection connection = Conexion.GetConecctionManttoCE();
            SqlDataReader dataReader;
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                string miQry = "SELECT Id, Tema,Orden,TemaStr FROM Temas WHERE idProd = @idProd ORDER BY TemaStr";
                cmd = new SqlCommand(miQry, connection);
                cmd.Parameters.AddWithValue("@IdProd", idProducto);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Temas tema = new Temas();
                    tema.IdTema = Convert.ToInt32(dataReader["Id"]);
                    tema.Tema = dataReader["Tema"].ToString();
                    tema.Orden = Convert.ToInt32(dataReader["Orden"]);
                    tema.TemaStr = dataReader["TemaSTR"].ToString();

                    temas.Add(tema);
                }
                dataReader.Close();

                this.InsertaTemas(temas);
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

            return temas.Count;
        }

        /// <summary>
        /// Inserta los temas del producto que se esta migrando a su respectiva base de datos de access
        /// </summary>
        /// <param name="temas"></param>
        private void InsertaTemas(List<Temas> temas)
        {
            OleDbConnection connection = Conexion.GetAccessDataBaseConnection(idProducto);
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            string sqlCadena = "SELECT * FROM Temas WHERE id = 0";

            try
            {
                connection.Open();

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Temas");

                foreach (Temas tema in temas)
                {


                    dr = dataSet.Tables["Temas"].NewRow();
                    dr["Id"] = tema.IdTema;
                    dr["Desdoblamiento"] = tema.Tema;
                    dr["orden"] = tema.Orden;
                    dr["TemaSTR"] = tema.TemaStr;

                    dataSet.Tables["Temas"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connection.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO Temas(Id,Desdoblamiento,Orden,TemaStr)" +
                        " VALUES(@Id,@Desdoblamiento,@Orden,@TemaStr)";

                    dataAdapter.InsertCommand.Parameters.Add("@Id", OleDbType.Numeric, 0, "Id");
                    dataAdapter.InsertCommand.Parameters.Add("@Desdoblamiento", OleDbType.LongVarChar, 0, "Desdoblamiento");
                    dataAdapter.InsertCommand.Parameters.Add("@Orden", OleDbType.Numeric, 0, "Orden");
                    dataAdapter.InsertCommand.Parameters.Add("@TemaStr", OleDbType.LongVarChar, 0, "TemaStr");

                    dataAdapter.Update(dataSet, "Temas");

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                }


            }
            catch (OleDbException ex)
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

        #endregion

        #region Relaciones

        /// <summary>
        /// Obtienen las relaciones Temas-Tesis establecidas en el programa de mantenimiento y las envia
        /// al método Insertar
        /// </summary>
        /// <returns></returns>
        public int GetRelaciones()
        {
            List<Temas> temas = new List<Temas>();

            SqlConnection connection = Conexion.GetConecctionManttoCE();
            SqlDataReader dataReader;
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                string miQry = "SELECT * FROM TemasIUS WHERE idProd = @idProd";
                
                cmd = new SqlCommand(miQry, connection);
                cmd.Parameters.AddWithValue("@idProd", idProducto);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Temas tema = new Temas();
                    tema.IdTema = Convert.ToInt32(dataReader["Id"]);
                    tema.Nivel = Convert.ToInt32(dataReader["IUS"]);
                    tema.Padre = Convert.ToInt32(dataReader["ConsecIndx"]);

                    temas.Add(tema);
                }
                dataReader.Close();

                this.InsertaTemasIus(temas);
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

            return temas.Count;
        }

        /// <summary>
        /// Inserta las relaciones Temas-IUS dentro de su respectiva base de datos de access
        /// </summary>
        /// <param name="temas"></param>
        private void InsertaTemasIus(List<Temas> temas)
        {
            OleDbConnection connection = Conexion.GetAccessDataBaseConnection(idProducto);
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            string sqlCadena = "SELECT * FROM TemasIUS WHERE id = 0";

            try
            {
                connection.Open();

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Temas");

                foreach (Temas tema in temas)
                {
                    dr = dataSet.Tables["Temas"].NewRow();
                    dr["Id"] = tema.IdTema;
                    dr["IUS"] = tema.Nivel;
                    dr["ConsecIndx"] = tema.Padre;

                    dataSet.Tables["Temas"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connection.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO TemasIUS(Id,IUS,ConsecIndx)" +
                        " VALUES(@Id,@IUS,@ConsecIndx)";

                    dataAdapter.InsertCommand.Parameters.Add("@Id", OleDbType.Numeric, 0, "Id");
                    dataAdapter.InsertCommand.Parameters.Add("@IUS", OleDbType.Numeric, 0, "IUS");
                    dataAdapter.InsertCommand.Parameters.Add("@ConsecIndx", OleDbType.Numeric, 0, "ConsecIndx");

                    dataAdapter.Update(dataSet, "Temas");

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                }
            }
            catch (OleDbException ex)
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


        #endregion

    }
}
