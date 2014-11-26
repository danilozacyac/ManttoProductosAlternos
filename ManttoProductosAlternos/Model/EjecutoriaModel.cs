using System;
using System.Collections.Generic;
using ManttoProductosAlternos.DTO;
using System.Data.SqlClient;
using ManttoProductosAlternos.DBAccess;
using System.Data;
using System.Data.Common;
using ManttoProductosAlternos.Interface;
using ScjnUtilities;
using System.Windows.Forms;

namespace ManttoProductosAlternos.Model
{
    public class EjecutoriaModel : IDocumentos
    {
        
        /// <summary>
        /// Trae la información de las ejecutorias a partir de su número de IUS
        /// </summary>
        /// <param name="ius"></param>
        /// <returns></returns>
        public DocumentoTO GetDocumentoPorIus(long ius)
        {
            SqlConnection conneDsql = Conexion.GetConnectionCt9bd3();
            SqlCommand cmd;
            SqlDataReader dataReader;

            DocumentoTO ejecutoria = null;

            try
            {
                conneDsql.Open();
                string sqlCadena = "SELECT E.* FROM Ejecutoria E  WHERE E.Id = @ius";

                cmd = new SqlCommand(sqlCadena, conneDsql);
                cmd.Parameters.AddWithValue("@ius", ius);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    ejecutoria = new DocumentoTO();
                    ejecutoria.Id = Convert.ToInt32(dataReader["Id"].ToString());
                    ejecutoria.Rubro = dataReader["Rubro"].ToString();
                    ejecutoria.ConsecIndx = Convert.ToInt32(dataReader["ConsecIndx"].ToString());
                    ejecutoria.LocExp = dataReader["LocExp"].ToString();
                    ejecutoria.LocAbr = dataReader["LocAbr"].ToString();
                    ejecutoria.Parte = Convert.ToInt32(dataReader["Parte"].ToString());
                    ejecutoria.Asunto = dataReader["Asunto"].ToString();
                    ejecutoria.Promovente = dataReader["Promovente"].ToString();
                    ejecutoria.AsuntoIndx = dataReader["API"].ToString();
                    ejecutoria.DatosAsuntoIndx = dataReader["MI"].ToString();
                    ejecutoria.LocIndx = dataReader["LI"].ToString();
                    ejecutoria.Volumen = Convert.ToInt32(dataReader["Volumen"].ToString());
                    ejecutoria.Consec = Convert.ToInt32(dataReader["Consec"].ToString());
                    ejecutoria.Tesis = dataReader["Tesis"].ToString();
                    ejecutoria.Sala = Convert.ToInt32(dataReader["Sala"].ToString());
                    ejecutoria.Epoca = Convert.ToInt32(dataReader["Epoca"].ToString());
                    ejecutoria.Fuente = Convert.ToInt32(dataReader["Fuente"].ToString());
                    ejecutoria.Pagina = dataReader["Pagina"].ToString();
                }
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
                conneDsql.Close();
            }
            return ejecutoria;
        }


        public void SetDocumento(DocumentoTO ejecutoriaDto,int idTipoEje)
        {
            SqlConnection connection = Conexion.GetConecctionManttoCE();
            SqlDataAdapter dataAdapter;
            SqlCommand cmd;
            SqlDataReader reader;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                connection.Open();
                string miQry = "SELECT * FROM Ejecutoria Where Id = " + ejecutoriaDto.Id;
                cmd = new SqlCommand(miQry, connection);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();
                    string sqlCadena = "SELECT * FROM Ejecutoria WHERE id = 0";


                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                    dataAdapter.Fill(dataSet, "Ejecutoria");

                    dr = dataSet.Tables["Ejecutoria"].NewRow();
                    dr["Id"] = ejecutoriaDto.Id;
                    dr["Rubro"] = ejecutoriaDto.Rubro;
                    dr["ConsecIndx"] = ejecutoriaDto.ConsecIndx;
                    dr["LocExp"] = ejecutoriaDto.LocExp;
                    dr["LocAbr"] = ejecutoriaDto.LocAbr;
                    dr["Parte"] = ejecutoriaDto.Parte;
                    dr["Asunto"] = ejecutoriaDto.Asunto;
                    dr["Promovente"] = ejecutoriaDto.Promovente;
                    dr["API"] = ejecutoriaDto.AsuntoIndx;
                    dr["MI"] = ejecutoriaDto.DatosAsuntoIndx;
                    dr["LI"] = ejecutoriaDto.LocIndx;
                    dr["Volumen"] = ejecutoriaDto.Volumen;
                    dr["Consec"] = ejecutoriaDto.Consec;
                    dr["tesis"] = ejecutoriaDto.Tesis;
                    dr["Sala"] = ejecutoriaDto.Sala;
                    dr["Epoca"] = ejecutoriaDto.Epoca;
                    dr["Fuente"] = ejecutoriaDto.Fuente;
                    dr["Pagina"] = ejecutoriaDto.Pagina;
                    dr["IdProd"] = idTipoEje;

                    dataSet.Tables["Ejecutoria"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connection.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO Ejecutoria(Id,Rubro,ConsecIndx,LocExp,LocAbr,Parte,Asunto,Promovente,API,MI,LI," +
                        "Volumen,Consec,Tesis,Sala,Epoca,Fuente,Pagina,IdProd)" +
                        " VALUES(@Id,@Rubro,@ConsecIndx,@LocExp,@LocAbr,@Parte,@Asunto,@Promovente,@API,@MI,@LI," +
                        "@Volumen,@Consec,@Tesis,@Sala,@Epoca,@Fuente,@Pagina,@IdProd)";

                    dataAdapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                    dataAdapter.InsertCommand.Parameters.Add("@Rubro", SqlDbType.NText, 0, "Rubro");
                    dataAdapter.InsertCommand.Parameters.Add("@ConsecIndx", SqlDbType.Int, 0, "ConsecIndx");
                    dataAdapter.InsertCommand.Parameters.Add("@locExp", SqlDbType.NText, 0, "LocExp");
                    dataAdapter.InsertCommand.Parameters.Add("@LocAbr", SqlDbType.NVarChar, 0, "LocAbr");
                    dataAdapter.InsertCommand.Parameters.Add("@Parte", SqlDbType.TinyInt, 0, "Parte");
                    dataAdapter.InsertCommand.Parameters.Add("@Asunto", SqlDbType.NVarChar, 0, "Asunto");
                    dataAdapter.InsertCommand.Parameters.Add("@Promovente", SqlDbType.NText, 0, "Promovente");
                    dataAdapter.InsertCommand.Parameters.Add("@API", SqlDbType.NText, 0, "API");
                    dataAdapter.InsertCommand.Parameters.Add("@MI", SqlDbType.NText, 0, "MI");
                    dataAdapter.InsertCommand.Parameters.Add("@LI", SqlDbType.NText, 0, "LI");
                    dataAdapter.InsertCommand.Parameters.Add("@Volumen", SqlDbType.SmallInt, 0, "Volumen");
                    dataAdapter.InsertCommand.Parameters.Add("@Consec", SqlDbType.SmallInt, 0, "Consec");
                    dataAdapter.InsertCommand.Parameters.Add("@Tesis", SqlDbType.NVarChar, 0, "Tesis");
                    dataAdapter.InsertCommand.Parameters.Add("@Sala", SqlDbType.TinyInt, 0, "Sala");
                    dataAdapter.InsertCommand.Parameters.Add("@Epoca", SqlDbType.TinyInt, 0, "Epoca");
                    dataAdapter.InsertCommand.Parameters.Add("@Fuente", SqlDbType.TinyInt, 0, "Fuente");
                    dataAdapter.InsertCommand.Parameters.Add("@Pagina", SqlDbType.NVarChar, 0, "Pagina");
                    dataAdapter.InsertCommand.Parameters.Add("@IdProd", SqlDbType.Int, 0, "IdProd");

                    dataAdapter.Update(dataSet, "Ejecutoria");

                    dataSet.Dispose();
                    dataAdapter.Dispose();
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
        }

        /// <summary>
        /// Obtiene las ejecutorias que fueron relacionadas con alguno de los apartados de Facultades
        /// de la Suprema Corte 
        /// </summary>
        /// <param name="ius"></param>
        /// <returns></returns>
        public List<DocumentoTO> GetDocumentosRelacionados(int idTipoEje)
        {
            SqlConnection connection = Conexion.GetConecctionManttoCE();
            SqlCommand cmd;
            SqlDataReader reader;

            List<DocumentoTO> ejecutorias = new List<DocumentoTO>();

            try
            {
                connection.Open();
                string sqlCadena = "SELECT Id,Rubro,Asunto,Promovente FROM Ejecutoria WHERE idProd = @idTipoEje ORDER BY ConsecIndx";

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@idTipoEje", idTipoEje);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DocumentoTO ejecutoria = new DocumentoTO();
                    ejecutoria.Id = Convert.ToInt32(reader["Id"].ToString());
                    ejecutoria.Rubro = reader["Rubro"].ToString();
                    ejecutoria.Asunto = reader["Asunto"].ToString();
                    ejecutoria.Promovente = reader["Promovente"].ToString();

                    ejecutorias.Add(ejecutoria);
                }
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

            return ejecutorias;
        }

        public void DeleteDocumento(long ius, int idTipoEje)
        {
            SqlConnection connection = Conexion.GetConecctionManttoCE();
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM Ejecutoria WHERE Id = " + ius + " AND idProd = " + idTipoEje;
                cmd.ExecuteNonQuery();

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

        /*
         * Los métodos que se encuentran debajo de este comentario permiten traer la información de las 
         * ejecutorias y votos relacionados con las tesis que se van asignando a cada tema de acuerdo 
         * a la "materia" de que se trate (suspensión,improcedencia,electoral,etc)
         * 
         * Actualmente están en deshuso pues se determino que ya no habría consultas a partir de ejecutorias
         * y que al estar la información inmersa dentro de la base de datos del ius era inecesario estar 
         * duplicando la información de las relaciones de cada tesis
         * */

        /*

        /// <summary>
        /// Obtiene la o las ejecutorias relacionadas con una Tesis a partir de su número de IUS
        /// </summary>
        /// <param name="ius"></param>
        /// <returns>Lista de Ejecutorias</returns>
        public List<EjecutoriaDTO> getEjecutoriasRelacionadas1(long ius)
        {
            SqlConnection conneDSQL = (SqlConnection)Conexion.GetConecctionDsql();
            SqlCommand cmd;
            SqlDataReader dataReader;

            List<EjecutoriaDTO> Ejecutorias = new List<EjecutoriaDTO>();

            try
            {
                conneDSQL.Open();
                string sqlCadena = "SELECT E.* FROM Ejecutoria E INNER JOIN Eje_IUS  EI  ON EI.ID = E.ID WHERE EI.Tpo = 2 and EI.IUS = " + ius;

                cmd = new SqlCommand(sqlCadena, conneDSQL);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    EjecutoriaDTO ejecutoria = new EjecutoriaDTO();
                    ejecutoria.IdTema = Convert.ToInt32(dataReader["IdTema"].ToString());
                    ejecutoria.Rubro = dataReader["Rubro"].ToString();
                    ejecutoria.ConsecIndx = Convert.ToInt32(dataReader["ConsecIndx"].ToString());
                    ejecutoria.LocExp = dataReader["LocExp"].ToString();
                    ejecutoria.LocAbr = dataReader["LocAbr"].ToString();
                    ejecutoria.Parte = Convert.ToInt32(dataReader["Parte"].ToString());
                    ejecutoria.Asunto = dataReader["Asunto"].ToString();
                    ejecutoria.Promovente = dataReader["Promovente"].ToString();
                    ejecutoria.AsuntoIndx = dataReader["API"].ToString();
                    ejecutoria.DatosAsuntoIndx = dataReader["MI"].ToString();
                    ejecutoria.LocIndx = dataReader["LI"].ToString();
                    ejecutoria.Volumen = Convert.ToInt32(dataReader["Volumen"].ToString());
                    ejecutoria.Consec = Convert.ToInt32(dataReader["Consec"].ToString());
                    ejecutoria.Tesis = dataReader["Tesis"].ToString();
                    ejecutoria.Sala = Convert.ToInt32(dataReader["Sala"].ToString());
                    ejecutoria.Epoca = Convert.ToInt32(dataReader["Epoca"].ToString());
                    ejecutoria.Fuente = Convert.ToInt32(dataReader["Fuente"].ToString());
                    ejecutoria.Pagina = dataReader["Pagina"].ToString();

                    Ejecutorias.Add(ejecutoria);
                }
            }
            catch (SqlException sql)
            {
                MessageBoxResult result = MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception ex)
            {
                MessageBoxResult result = MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                conneDSQL.Close();
            }



            return Ejecutorias;
        }

        public List<EjecutoriaDTO> getPartesEjecutoria(long ius)
        {
            SqlConnection conneDSQL = (SqlConnection)Conexion.GetConecctionDsql();
            SqlCommand cmd;
            SqlDataReader dataReader;

            List<EjecutoriaDTO> Ejecutorias = new List<EjecutoriaDTO>();

            try
            {
                conneDSQL.Open();
                string sqlCadena = "SELECT * FROM ParteEjecutoria WHERE ID = " + ius;

                cmd = new SqlCommand(sqlCadena, conneDSQL);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    EjecutoriaDTO ejecutoria = new EjecutoriaDTO();
                    ejecutoria.IdTema = Convert.ToInt32(dataReader["IdTema"].ToString());
                    ejecutoria.Consec = Convert.ToInt32(dataReader["Consec"].ToString());
                    ejecutoria.TextoParte = dataReader["txtParte"].ToString();
                    ejecutoria.TextoIndx = dataReader["TI"].ToString();
                    ejecutoria.TontaUnica = Convert.ToInt32(dataReader["TontaUnica"].ToString());
                    ejecutoria.Parte = Convert.ToInt32(dataReader["Parte"].ToString());
                    ejecutoria.ConsecIndx = Convert.ToInt32(dataReader["ConsecIndx"].ToString());

                    Ejecutorias.Add(ejecutoria);
                }
            }
            catch (SqlException sql)
            {
                MessageBoxResult result = MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception ex)
            {
                MessageBoxResult result = MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                conneDSQL.Close();
            }

            return Ejecutorias;
        }

        public void setEjecutoriasRelacionadas(List<EjecutoriaDTO> Ejecutorias, long ius)
        {
            SqlConnection connectionCT9BD2 = (SqlConnection)Conexion.GetConecctionManttoCE();
            DbDataAdapter dataAdapter;
            SqlCommand cmd;
            SqlDataReader dataReader;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                connectionCT9BD2.Open();
                foreach (EjecutoriaDTO ejecutoriaDTO in Ejecutorias)
                {
                    string miQry = "SELECT * FROM Ejecutoria Where IdTema = " + ejecutoriaDTO.IdTema;
                    cmd = new SqlCommand(miQry, connectionCT9BD2);
                    dataReader = cmd.ExecuteReader();

                    if (!dataReader.HasRows)
                    {
                        dataReader.Close();
                        string sqlCadena = "SELECT * FROM Ejecutoria WHERE id = 0";


                        dataAdapter = new SqlDataAdapter();
                        dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionCT9BD2);

                        dataAdapter.Fill(dataSet, "Ejecutoria");

                        dr = dataSet.Tables["Ejecutoria"].NewRow();
                        dr["IdTema"] = ejecutoriaDTO.IdTema;
                        dr["Rubro"] = ejecutoriaDTO.Rubro;
                        dr["ConsecIndx"] = ejecutoriaDTO.ConsecIndx;
                        dr["LocExp"] = ejecutoriaDTO.LocExp;
                        dr["LocAbr"] = ejecutoriaDTO.LocAbr;
                        dr["Parte"] = ejecutoriaDTO.Parte;
                        dr["Asunto"] = ejecutoriaDTO.Asunto;
                        dr["Promovente"] = ejecutoriaDTO.Promovente;
                        dr["API"] = ejecutoriaDTO.AsuntoIndx;
                        dr["MI"] = ejecutoriaDTO.DatosAsuntoIndx;
                        dr["LI"] = ejecutoriaDTO.LocIndx;
                        dr["Volumen"] = ejecutoriaDTO.Volumen;
                        dr["Consec"] = ejecutoriaDTO.Consec;
                        dr["tesis"] = ejecutoriaDTO.Tesis;
                        dr["Sala"] = ejecutoriaDTO.Sala;
                        dr["Epoca"] = ejecutoriaDTO.Epoca;
                        dr["Fuente"] = ejecutoriaDTO.Fuente;
                        dr["Pagina"] = ejecutoriaDTO.Pagina;
                        dr["IdProducto"] = 2;

                        dataSet.Tables["Ejecutoria"].Rows.Add(dr);

                        dataAdapter.InsertCommand = connectionCT9BD2.CreateCommand();
                        dataAdapter.InsertCommand.CommandText = "INSERT INTO Ejecutoria(IdTema,Rubro,ConsecIndx,LocExp,LocAbr,Parte,Asunto,Promovente,API,MI,LI," +
                            "Volumen,Consec,Tesis,Sala,Epoca,Fuente,Pagina,IdProducto)" +
                            " VALUES(@IdTema,@Rubro,@ConsecIndx,@LocExp,@LocAbr,@Parte,@Asunto,@Promovente,@API,@MI,@LI," +
                            "@Volumen,@Consec,@Tesis,@Sala,@Epoca,@Fuente,@Pagina,@IdProducto)";

                        dataAdapter.InsertCommand.Parameters.Add("@IdTema", SqlDbType.Int, 0, "IdTema");
                        dataAdapter.InsertCommand.Parameters.Add("@Rubro", SqlDbType.NText, 0, "Rubro");
                        dataAdapter.InsertCommand.Parameters.Add("@ConsecIndx", SqlDbType.Int, 0, "ConsecIndx");
                        dataAdapter.InsertCommand.Parameters.Add("@locExp", SqlDbType.NText, 0, "LocExp");
                        dataAdapter.InsertCommand.Parameters.Add("@LocAbr", SqlDbType.NVarChar, 0, "LocAbr");
                        dataAdapter.InsertCommand.Parameters.Add("@Parte", SqlDbType.TinyInt, 0, "Parte");
                        dataAdapter.InsertCommand.Parameters.Add("@Asunto", SqlDbType.NVarChar, 0, "Asunto");
                        dataAdapter.InsertCommand.Parameters.Add("@Promovente", SqlDbType.NText, 0, "Promovente");
                        dataAdapter.InsertCommand.Parameters.Add("@API", SqlDbType.NText, 0, "API");
                        dataAdapter.InsertCommand.Parameters.Add("@MI", SqlDbType.NText, 0, "MI");
                        dataAdapter.InsertCommand.Parameters.Add("@LI", SqlDbType.NText, 0, "LI");
                        dataAdapter.InsertCommand.Parameters.Add("@Volumen", SqlDbType.SmallInt, 0, "Volumen");
                        dataAdapter.InsertCommand.Parameters.Add("@Consec", SqlDbType.SmallInt, 0, "Consec");
                        dataAdapter.InsertCommand.Parameters.Add("@Tesis", SqlDbType.NVarChar, 0, "Tesis");
                        dataAdapter.InsertCommand.Parameters.Add("@Sala", SqlDbType.TinyInt, 0, "Sala");
                        dataAdapter.InsertCommand.Parameters.Add("@Epoca", SqlDbType.TinyInt, 0, "Epoca");
                        dataAdapter.InsertCommand.Parameters.Add("@Fuente", SqlDbType.TinyInt, 0, "Fuente");
                        dataAdapter.InsertCommand.Parameters.Add("@Pagina", SqlDbType.NVarChar, 0, "Pagina");
                        dataAdapter.InsertCommand.Parameters.Add("@IdProducto", SqlDbType.Int, 0, "IdProducto");

                        dataAdapter.Update(dataSet, "Ejecutoria");

                        dataSet.Dispose();
                        dataAdapter.Dispose();

                        setPartesEjecutorias(getPartesEjecutoria(ejecutoriaDTO.IdTema));

                    }
                    dataReader.Close();
                }

            }
            catch (SqlException sql)
            {
                MessageBoxResult result = MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            finally
            {
                connectionCT9BD2.Close();
            }
        }

        public void setPartesEjecutorias(List<EjecutoriaDTO> Ejecutorias)
        {
            SqlConnection connectionEpsSQL = new SqlConnection();
            DbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;


            string sqlCadena = "SELECT * FROM ParteEjecutoria WHERE id = 0";

            connectionEpsSQL = (SqlConnection)Conexion.GetConecctionManttoCE();

            try
            {
                foreach (EjecutoriaDTO ejecutoriaDTO in Ejecutorias)
                {
                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionEpsSQL);

                    dataAdapter.Fill(dataSet, "Ejecutoria");

                    dr = dataSet.Tables["Ejecutoria"].NewRow();
                    dr["IdTema"] = ejecutoriaDTO.IdTema;
                    dr["Consec"] = ejecutoriaDTO.Consec;
                    dr["txtParte"] = ejecutoriaDTO.TextoParte;
                    dr["TI"] = ejecutoriaDTO.TextoIndx;
                    dr["TontaUnica"] = ejecutoriaDTO.TontaUnica;
                    dr["Parte"] = ejecutoriaDTO.Parte;
                    dr["ConsecIndx"] = ejecutoriaDTO.ConsecIndx;

                    dataSet.Tables["Ejecutoria"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connectionEpsSQL.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO ParteEjecutoria(IdTema,Consec,txtParte,TI,TontaUnica,Parte,ConsecIndx)" +
                        " VALUES(@IdTema,@Consec,@txtParte,@TI,@TontaUnica,@Parte,@ConsecIndx)";

                    dataAdapter.InsertCommand.Parameters.Add("@IdTema", SqlDbType.Int, 0, "IdTema");
                    dataAdapter.InsertCommand.Parameters.Add("@Consec", SqlDbType.Int, 0, "Consec");
                    dataAdapter.InsertCommand.Parameters.Add("@txtParte", SqlDbType.NText, 0, "txtParte");
                    dataAdapter.InsertCommand.Parameters.Add("@TI", SqlDbType.NText, 0, "TI");
                    dataAdapter.InsertCommand.Parameters.Add("@TontaUnica", SqlDbType.SmallInt, 0, "TontaUnica");
                    dataAdapter.InsertCommand.Parameters.Add("@Parte", SqlDbType.TinyInt, 0, "Parte");
                    dataAdapter.InsertCommand.Parameters.Add("@ConsecIndx", SqlDbType.Int, 0, "ConsecIndx");

                    dataAdapter.Update(dataSet, "Ejecutoria");

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                }


            }
            catch (SqlException sql)
            {
                MessageBoxResult result = MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            finally
            {
                connectionEpsSQL.Close();
            }
        }
        */
    }
}
