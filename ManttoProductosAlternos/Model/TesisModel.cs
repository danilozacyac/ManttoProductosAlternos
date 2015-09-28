using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using ManttoProductosAlternos.DBAccess;
using ManttoProductosAlternos.Dto;
using ScjnUtilities;

namespace ManttoProductosAlternos.Model
{
    public class TesisModel
    {
        private readonly int idProducto = 0;

        public TesisModel(int idProducto)
        {
            this.idProducto = idProducto;
        }

        /// <summary>
        /// Devuelve las tesis relacionadas con el tema seleccionado
        /// </summary>
        /// <param name="idTema">Identificador del tema seleccionado</param>
        /// <returns></returns>
        public List<TesisDTO> GetTesisRelacionadas(int idTema)
        {
            List<TesisDTO> listaTesis = new List<TesisDTO>();
            SqlConnection connection = Conexion.GetConecctionManttoCE();

            SqlDataReader dataReader;
            SqlCommand cmdAntes;

            cmdAntes = connection.CreateCommand();
            cmdAntes.Connection = connection;

            try
            {
                connection.Open();

                string miQry = "SELECT T.IUS,T.Rubro,T.Texto,T.Prec,T.LocAbr,T.LocExp,T.Volumen,T.Parte " +
                               " FROM Tesis T INNER JOIN TemasIUS I ON I.IUS = T.IUS  " +
                               " WHERE (I.Id = @idTema AND T.IdProd = @idProducto ) AND I.idProd = @idProducto " +
                               " ORDER BY T.ConsecIndx";

                cmdAntes = new SqlCommand(miQry, connection);
                cmdAntes.Parameters.AddWithValue("@idTema", idTema);
                cmdAntes.Parameters.AddWithValue("@idProducto", idProducto);
                dataReader = cmdAntes.ExecuteReader();

                while (dataReader.Read())
                {
                    TesisDTO tesis = new TesisDTO();
                    tesis.Ius = Convert.ToInt32(dataReader["IUS"].ToString());
                    tesis.Rubro = dataReader["Rubro"].ToString();
                    tesis.Texto = dataReader["Texto"].ToString();
                    tesis.Precedente = dataReader["Prec"].ToString();
                    tesis.LocAbr = dataReader["LocAbr"].ToString();
                    tesis.LocExpr = dataReader["LocExp"].ToString();
                    tesis.Volumen = Convert.ToInt32(dataReader["Volumen"].ToString());
                    tesis.Parte = Convert.ToInt32(dataReader["Parte"].ToString());

                    listaTesis.Add(tesis);
                }
                dataReader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "ManttoProductosAlternos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "ManttoProductosAlternos");
            }
            finally
            {
                connection.Close();
            }

            return listaTesis;
        }

        public List<int> GetRegistrosRelacionados(int idTema)
        {
            List<int> listaTesis = new List<int>();
            SqlConnection connection = Conexion.GetConecctionManttoCE();

            SqlDataReader dataReader;
            SqlCommand cmdAntes;

            cmdAntes = connection.CreateCommand();
            cmdAntes.Connection = connection;

            try
            {
                connection.Open();

                string miQry = "SELECT IUS FROM TemasIUS WHERE Id = @idTema AND IdProd = @idProducto ORDER BY ConsecIndx";

                cmdAntes = new SqlCommand(miQry, connection);
                cmdAntes.Parameters.AddWithValue("@idTema", idTema);
                cmdAntes.Parameters.AddWithValue("@idProducto", idProducto);
                dataReader = cmdAntes.ExecuteReader();

                while (dataReader.Read())
                {
                    listaTesis.Add(dataReader["IUS"] as int? ?? -1);
                }
                dataReader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "ManttoProductosAlternos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "ManttoProductosAlternos");
            }
            finally
            {
                connection.Close();
            }

            return listaTesis;
        }

        /// <summary>
        /// Devuelve todas las tesis relacionadas al producto que se esta trabajando
        /// </summary>
        /// <returns></returns>
        public List<TesisDTO> GetTesisPorTema()
        {
            List<TesisDTO> listaTesis = new List<TesisDTO>();
            SqlConnection connection = Conexion.GetConecctionManttoCE();

            SqlDataReader dataReader;
            SqlCommand cmdAntes;

            cmdAntes = connection.CreateCommand();
            cmdAntes.Connection = connection;

            try
            {
                connection.Open();

                string miQry = "SELECT T.IUS,T.Rubro,T.Texto,T.Prec,T.LocAbr,T.LocExp,T.Volumen,T.Parte " +
                               " FROM Tesis T " +
                               " WHERE T.idProd = @idProducto " +
                               " ORDER BY T.ConsecIndx";

                cmdAntes = new SqlCommand(miQry, connection);
                cmdAntes.Parameters.AddWithValue("@idProducto", idProducto);
                dataReader = cmdAntes.ExecuteReader();

                while (dataReader.Read())
                {
                    TesisDTO tesis = new TesisDTO();
                    tesis.Ius = Convert.ToInt32(dataReader["IUS"].ToString());
                    tesis.Rubro = dataReader["Rubro"].ToString();
                    tesis.Texto = dataReader["Texto"].ToString();
                    tesis.Precedente = dataReader["Prec"].ToString();
                    tesis.LocAbr = dataReader["LocAbr"].ToString();
                    tesis.LocExpr = dataReader["LocExp"].ToString();
                    tesis.Volumen = Convert.ToInt32(dataReader["Volumen"].ToString());
                    tesis.Parte = Convert.ToInt32(dataReader["Parte"].ToString());

                    listaTesis.Add(tesis);
                }
                dataReader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "ManttoProductosAlternos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "ManttoProductosAlternos");
            }
            finally
            {
                connection.Close();
            }
            return listaTesis;
        }

        public void InsertaTesis(long ius, int idTema)
        {
            SqlConnection connection = Conexion.GetConnectionCt9bd3();
            SqlConnection b2Conne = null;

            SqlCommand sqlCmd;
            SqlDataReader dataReader;

            sqlCmd = connection.CreateCommand();
            sqlCmd.Connection = connection;

            try
            {
                SqlConnection conneMant = Conexion.GetConnectMantesis();
                SqlCommand oleCmd;
                string miQry = "SELECT * FROM OtrosTExtos WHERE IUS = " + ius;
                string nota = String.Empty;

                conneMant.Open();
                SqlDataReader oleDR;
                oleCmd = new SqlCommand(miQry, conneMant);
                oleDR = oleCmd.ExecuteReader();

                if (oleDR.HasRows)
                {
                    oleDR.Read();
                    nota = oleDR["Nota"].ToString();
                }

                connection.Open();

                miQry = "SELECT T.IUS,T.Consec,T.Rubro,T.Texto,T.Prec,T.IdGenealogia,T.LocAbr,T.LocExp,T.ConsecIndx,T.Volumen,T.Parte, " +
                        "T.RIndx,T.TIndx,T.PIndx,T.LIndx,T.Tesis " +
                        "FROM Tesis T WHERE T.IUS = " + ius;
                sqlCmd = new SqlCommand(miQry, connection);
                dataReader = sqlCmd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    b2Conne = Conexion.GetConecctionManttoCE();
                    SqlCommand b2Cmd;

                    b2Cmd = b2Conne.CreateCommand();
                    b2Cmd.Connection = b2Conne;

                    b2Conne.Open();

                    b2Cmd.CommandText = "INSERT INTO TemasIUS VALUES(" + idTema + "," + ius + "," + dataReader["ConsecIndx"].ToString() + "," + idProducto + ")";
                    b2Cmd.ExecuteNonQuery();

                    b2Cmd.CommandText = "insert into Bitacora(idTema,tipoModif,edoAnterior,usuario,idProd)" +
                                        "values(" + idTema + ",11,'" + idTema + "-" + ius + " ','" + Environment.MachineName + "'," + idProducto + ")";
                    b2Cmd.ExecuteNonQuery();

                    InsertaTesisParametro(dataReader);

                    if (!nota.Equals(String.Empty))
                    {
                        b2Cmd.CommandText = "INSERT INTO Notas(IUS,Nota) VALUES(" + dataReader["IUS"].ToString() + ",'" + nota + "')";
                        b2Cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    throw new ArgumentNullException("Reader", "No se encontro ninguna tesis con el número IUS especificado");
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (ArgumentNullException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                connection.Close();
                if (b2Conne != null)
                {
                    b2Conne.Close();
                }
            }
        }

        private bool InsertaTesisParametro(SqlDataReader dataReader)
        {
            try
            {
                SqlConnection connectionCT9BD2 = Conexion.GetConecctionManttoCE();
                DbDataAdapter dataAdapter;
                DataSet dataSet = new DataSet();
                DataRow dr;

                connectionCT9BD2.Open();
                string sqlCadena = "SELECT * FROM Tesis WHERE ius = 0";
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionCT9BD2);

                dataAdapter.Fill(dataSet, "Tesis");

                dr = dataSet.Tables["Tesis"].NewRow();
                dr["IUS"] = dataReader["IUS"].ToString();
                dr["Consec"] = dataReader["Consec"].ToString();
                dr["Rubro"] = dataReader["Rubro"].ToString();
                dr["Texto"] = dataReader["Texto"].ToString();
                dr["Prec"] = dataReader["prec"].ToString();
                dr["IdGenealogia"] = dataReader["IdGenealogia"].ToString();
                dr["ConsecIndx"] = dataReader["Consecindx"].ToString();
                dr["LocAbr"] = dataReader["LocAbr"].ToString();
                dr["LocExp"] = dataReader["LocExp"].ToString();
                dr["Volumen"] = dataReader["Volumen"].ToString();
                dr["RIndx"] = dataReader["RIndx"].ToString();
                dr["TIndx"] = dataReader["TIndx"].ToString();
                dr["PIndx"] = dataReader["PIndx"].ToString();
                dr["LIndx"] = dataReader["LIndx"].ToString();
                dr["Parte"] = dataReader["Parte"].ToString();
                dr["Tesis"] = dataReader["Tesis"].ToString();
                dr["idProd"] = idProducto;

                dataSet.Tables["Tesis"].Rows.Add(dr);

                dataAdapter.InsertCommand = connectionCT9BD2.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Tesis(IUS,Consec,Rubro,Texto,Prec,IdGenealogia,ConsecIndx,LocAbr,LocExp" +
                                                        ",Volumen,RIndx,TIndx,PIndx,LIndx,Parte,IdProd,Tesis)" +
                                                        " VALUES(@IUS,@Consec,@Rubro,@Texto,@Prec,@IdGenealogia,@ConsecIndx,@LocAbr,@LocExp" +
                                                        ",@Volumen,@RIndx,@TIndx,@PIndx,@LIndx,@Parte,@IdProd,@Tesis)";

                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IUS", SqlDbType.Int, 0, "IUS");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Consec", SqlDbType.Int, 0, "Consec");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Rubro", SqlDbType.NText, 0, "Rubro");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Texto", SqlDbType.NText, 0, "Texto");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Prec", SqlDbType.NText, 0, "Prec");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IdGenealogia", SqlDbType.Int, 0, "IdGenealogia");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@ConsecIndx", SqlDbType.Int, 0, "ConsecIndx");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@LocAbr", SqlDbType.NText, 0, "LocAbr");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@LocExp", SqlDbType.NText, 0, "LocExp");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Volumen", SqlDbType.Int, 0, "Volumen");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@RIndx", SqlDbType.NText, 0, "RIndx");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TIndx", SqlDbType.NText, 0, "Tindx");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@PIndx", SqlDbType.NText, 0, "PIndx");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@LIndx", SqlDbType.NVarChar, 0, "LIndx");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Parte", SqlDbType.Int, 0, "Parte");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Tesis", SqlDbType.VarChar, 0, "Tesis");
                ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@idProd", SqlDbType.Int, 0, "idProd");

                dataAdapter.Update(dataSet, "Tesis");

                dataSet.Dispose();
                dataAdapter.Dispose();
                return true;
            }
            catch (SqlException ex)
            {
                if (!ex.Message.Contains("UNIQUE"))
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                    MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
                }
                
                return false;
            }
        }

        public void EliminaRelacion(long ius, int idTema)
        {
            SqlConnection connection = Conexion.GetConecctionManttoCE();
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM TemasIUS WHERE id = " + idTema + " AND IUS = " + ius + "AND idProd = " + idProducto;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "insert into Bitacora(idTema,tipoModif,edoAnterior,usuario,idProd)" +
                                  "values(" + idTema + ",12,'" + idTema + "-" + ius + " ','" + Environment.MachineName + "'," + idProducto + ")";
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "ManttoProductosAlternos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "ManttoProductosAlternos");
            }
            finally
            {
                connection.Close();
            }
        }

        public void EliminaTesisLista(long ius)
        {
            SqlConnection connection = Conexion.GetConecctionManttoCE();

            SqlDataReader dataReader;
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                string miQry = "SELECT * FROM TemasIUS WHERE IUS = " + ius + " AND idProd = " + idProducto;

                cmd = new SqlCommand(miQry, connection);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    this.EliminaRelacion(ius, Convert.ToInt32(dataReader["id"]));
                }
                dataReader.Close();

                cmd.CommandText = "DELETE FROM Tesis Where ius = " + ius + " AND idProd = " + idProducto;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "ManttoProductosAlternos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TesisModel", "ManttoProductosAlternos");
            }
            finally
            {
                connection.Close();
            }
        }

        public void SetConsecIndx()
        {
            SqlConnection db2Conne = Conexion.GetConecctionManttoCE();
            SqlConnection sqlConne = Conexion.GetConnectionCt9bd3();
            SqlConnection db2InsertConne = Conexion.GetConecctionManttoCE();

            SqlCommand db2Cmd;
            SqlCommand sqlCmd;

            SqlDataReader db2DataReader;
            SqlDataReader sqlDataReader;

            db2Cmd = db2Conne.CreateCommand();
            db2Cmd.Connection = db2Conne;

            sqlCmd = sqlConne.CreateCommand();
            sqlCmd.Connection = sqlConne;

            try
            {
                db2Conne.Open();
                db2InsertConne.Open();

                string miQry = "SELECT IUS FROM Tesis ";
                db2Cmd = new SqlCommand(miQry, db2Conne);
                db2DataReader = db2Cmd.ExecuteReader();

                sqlConne.Open();
                while (db2DataReader.Read())
                {
                    miQry = "SELECT Consec,ConsecIndx FROM Tesis WHERE IUS = " + db2DataReader["IUS"].ToString();
                    sqlCmd = new SqlCommand(miQry, sqlConne);
                    sqlDataReader = sqlCmd.ExecuteReader();

                    if (sqlDataReader.HasRows)
                    {
                        sqlDataReader.Read();

                        SqlCommand bd2InsertCmd;
                        bd2InsertCmd = db2InsertConne.CreateCommand();
                        bd2InsertCmd.Connection = db2InsertConne;

                        bd2InsertCmd.CommandText = "UPDATE Tesis SET Consec = " + sqlDataReader["Consec"].ToString() +
                                                   ",ConsecIndx = " + sqlDataReader["ConsecIndx"].ToString() + " WHERE IUS = " + db2DataReader["IUS"].ToString();
                        bd2InsertCmd.ExecuteNonQuery();
                    }
                    sqlDataReader.Close();
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                db2Conne.Close();
                sqlConne.Close();
                db2InsertConne.Close();
            }
        }

        //public static void SetNumTesis()
        //{
        //    SqlConnection db2Conne = Conexion.GetConecctionManttoCE();
        //    SqlConnection sqlConne = Conexion.GetConnectionCt9bd3();
        //    SqlConnection db2InsertConne = Conexion.GetConecctionManttoCE();

        //    SqlCommand db2Cmd;
        //    SqlCommand sqlCmd;

        //    SqlDataReader db2DataReader;
        //    SqlDataReader sqlDataReader;

        //    db2Cmd = db2Conne.CreateCommand();
        //    db2Cmd.Connection = db2Conne;

        //    sqlCmd = sqlConne.CreateCommand();
        //    sqlCmd.Connection = sqlConne;

        //    try
        //    {
        //        db2Conne.Open();
        //        db2InsertConne.Open();

        //        string miQry = "SELECT IUS FROM Tesis ";
        //        db2Cmd = new SqlCommand(miQry, db2Conne);
        //        db2DataReader = db2Cmd.ExecuteReader();

        //        int cuenta = 0;
        //        sqlConne.Open();
        //        while (db2DataReader.Read())
        //        {
        //            miQry = "SELECT Tesis FROM Tesis WHERE IUS = " + db2DataReader["IUS"].ToString();
        //            sqlCmd = new SqlCommand(miQry, sqlConne);
        //            sqlDataReader = sqlCmd.ExecuteReader();

        //            if (sqlDataReader.HasRows)
        //            {
        //                sqlDataReader.Read();

        //                SqlCommand bd2InsertCmd;
        //                bd2InsertCmd = db2InsertConne.CreateCommand();
        //                bd2InsertCmd.Connection = db2InsertConne;

        //                bd2InsertCmd.CommandText = "UPDATE Tesis SET Tesis = '" + sqlDataReader["Tesis"].ToString() +
        //                                           "' WHERE IUS = " + db2DataReader["IUS"].ToString();
        //                bd2InsertCmd.ExecuteNonQuery();
        //            }
        //            sqlDataReader.Close();

        //            cuenta++;
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //        MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
        //    }
        //    finally
        //    {
        //        db2Conne.Close();
        //        sqlConne.Close();
        //        db2InsertConne.Close();
        //    }
        //}

        public bool SustituyeTesis(long iusViejo, long iusNuevo)
        {
            SqlConnection connection = Conexion.GetConnectionCt9bd3();
            SqlConnection b2Conne = null;

            SqlCommand sqlCmd;
            SqlDataReader dataReader;

            sqlCmd = connection.CreateCommand();
            sqlCmd.Connection = connection;

            bool result = true;

            try
            {
                SqlConnection conneMant = Conexion.GetConnectMantesis();
                SqlCommand oleCmd;
                string miQry = "SELECT * FROM OtrosTExtos WHERE IUS = " + iusNuevo;
                string nota = String.Empty;

                conneMant.Open();
                SqlDataReader oleDR;
                oleCmd = new SqlCommand(miQry, conneMant);
                oleDR = oleCmd.ExecuteReader();

                if (oleDR.HasRows)
                {
                    oleDR.Read();
                    nota = oleDR["Nota"].ToString();
                }

                connection.Open();

                miQry = "SELECT T.IUS,T.Consec,T.Rubro,T.Texto,T.Prec,T.IdGenealogia,T.LocAbr,T.LocExp,T.ConsecIndx,T.Volumen,T.Parte, " +
                        "T.RIndx,T.TIndx,T.PIndx,T.LIndx,T.Tesis " +
                        "FROM Tesis T WHERE T.IUS = " + iusNuevo;
                sqlCmd = new SqlCommand(miQry, connection);
                dataReader = sqlCmd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    b2Conne = Conexion.GetConecctionManttoCE();
                    SqlCommand b2Cmd;

                    b2Cmd = b2Conne.CreateCommand();
                    b2Cmd.Connection = b2Conne;

                    b2Conne.Open();

                    b2Cmd.CommandText = "insert into Bitacora(idTema,tipoModif,edoAnterior,usuario,idProd)" +
                                        "values(0,13,'" + iusViejo + "-" + iusNuevo + " ','" + Environment.MachineName + "'," + idProducto + ")";
                    b2Cmd.ExecuteNonQuery();

                    bool respuesta = InsertaTesisParametro(dataReader);

                    if (respuesta)
                        if (!nota.Equals(String.Empty))
                        {
                            b2Cmd.CommandText = "INSERT INTO Notas(IUS,Nota) VALUES(" + dataReader["IUS"].ToString() + ",'" + nota + "')";
                            b2Cmd.ExecuteNonQuery();
                        }

                    b2Cmd.CommandText = "DELETE FROM Tesis Where ius = " + iusViejo;// +" AND idProd = " + idProducto;
                    b2Cmd.ExecuteNonQuery();

                    dataReader.Close();
                    miQry = "SELECT * FROM TemasIUS WHERE IUS = " + iusViejo;
                    sqlCmd = new SqlCommand(miQry, b2Conne);
                    dataReader = sqlCmd.ExecuteReader();

                    Dictionary<long, long> actualizar = new Dictionary<long, long>();

                    while (dataReader.Read())
                    {
                        actualizar.Add(Convert.ToInt64(dataReader["Id"]), iusNuevo);
                    }
                    dataReader.Close();

                    foreach (KeyValuePair<long, long> dato in actualizar)
                    {
                        try
                        {
                            b2Cmd.CommandText = "UPDATE TemasIUS SET ius = " + iusNuevo + " WHERE ius = " + iusViejo + " AND id = " + dato.Key;
                            b2Cmd.ExecuteNonQuery();
                        }
                        catch (SqlException)
                        {
                        }
                    }

                    b2Cmd.CommandText = "DELETE FROM TemasIUS Where ius = " + iusViejo;
                    b2Cmd.ExecuteNonQuery();
                }
                else
                {
                    throw new ArgumentNullException("Reader", "No se encontro ninguna tesis con el número IUS especificado");
                }
                
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
                result = false;
            }
            catch (ArgumentNullException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
                result = false;
            }
            finally
            {
                connection.Close();
                if (b2Conne != null)
                {
                    b2Conne.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// Copia todas las tesis relacionadas de un tema a otro
        /// </summary>
        /// <param name="idTemaViejo">Tema de donde se toman las tesis relacionadas</param>
        /// <param name="idTemaNuevo">Tema donde se establecera la nueva relacion de las tesis </param>
        public void CopiaTesis(int idTemaViejo, int idTemaNuevo)
        {
            List<int> tesisCopiar = this.GetRegistrosRelacionados(idTemaViejo);

            foreach (int tesis in tesisCopiar)
                this.InsertaTesis(tesis, idTemaNuevo);
        }

        /// <summary>
        /// Elimina todas las tesis relacionadas a un tema y las relaciona con otro
        /// </summary>
        /// <param name="idTemaViejo">Tema de donde se tomaran y eliminaran las tesis relacionadas</param>
        /// <param name="idTemaNuevo">Tema donde se establecera la nueva relacion de las tesis</param>
        public void CortarTesis(int idTemaViejo, int idTemaNuevo)
        {
            List<int> tesisCortar = this.GetRegistrosRelacionados(idTemaViejo);

            foreach (int tesis in tesisCortar)
                this.InsertaTesis(tesis, idTemaNuevo);

            foreach (int tesis in tesisCortar)
                this.EliminaRelacion(tesis, idTemaViejo);
        }

        /// <summary>
        /// Elimina todas las relaciones tema-tesis del tema seleccionado
        /// </summary>
        /// <param name="idTema"></param>
        public void EliminaTesis(int idTema)
        {
            List<int> tesisEliminar = this.GetRegistrosRelacionados(idTema);

            foreach (int tesis in tesisEliminar)
                this.EliminaRelacion(tesis, idTema);
        }
    }
}