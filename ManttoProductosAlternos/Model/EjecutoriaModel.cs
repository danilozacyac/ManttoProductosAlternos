using System;
using System.Collections.Generic;
using ManttoProductosAlternos.DTO;
using System.Data.SqlClient;
using ManttoProductosAlternos.DBAccess;
using System.Windows;
using System.Data;
using System.Data.Common;
using ManttoProductosAlternos.Interface;

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
            SqlConnection conneDSQL = (SqlConnection)Conexion.GetConecctionDsql();
            SqlCommand cmd;
            SqlDataReader dataReader;

            DocumentoTO ejecutoria = null;

            try
            {
                conneDSQL.Open();
                string sqlCadena = "SELECT E.* FROM Ejecutoria E  WHERE E.Id = " + ius;

                cmd = new SqlCommand(sqlCadena, conneDSQL);
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
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                conneDSQL.Close();
            }
            return ejecutoria;
        }

        public void SetDocumento(DocumentoTO ejecutoriaDTO,int idTipoEje)
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
                string miQry = "SELECT * FROM Ejecutoria Where Id = " + ejecutoriaDTO.Id;
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
                    dr["Id"] = ejecutoriaDTO.Id;
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
                    dr["IdProd"] = idTipoEje;

                    dataSet.Tables["Ejecutoria"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connectionCT9BD2.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO Ejecutoria(Id,Rubro,ConsecIndx,LocExp,LocAbr,Parte,Asunto,Promovente,API,MI,LI," +
                        "Volumen,Consec,Tesis,Sala,Epoca,Fuente,Pagina,IdProd)" +
                        " VALUES(@Id,@Rubro,@ConsecIndx,@LocExp,@LocAbr,@Parte,@Asunto,@Promovente,@API,@MI,@LI," +
                        "@Volumen,@Consec,@Tesis,@Sala,@Epoca,@Fuente,@Pagina,@IdProd)";

                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Rubro", SqlDbType.NText, 0, "Rubro");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@ConsecIndx", SqlDbType.Int, 0, "ConsecIndx");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@locExp", SqlDbType.NText, 0, "LocExp");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@LocAbr", SqlDbType.NVarChar, 0, "LocAbr");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Parte", SqlDbType.TinyInt, 0, "Parte");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Asunto", SqlDbType.NVarChar, 0, "Asunto");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Promovente", SqlDbType.NText, 0, "Promovente");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@API", SqlDbType.NText, 0, "API");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@MI", SqlDbType.NText, 0, "MI");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@LI", SqlDbType.NText, 0, "LI");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Volumen", SqlDbType.SmallInt, 0, "Volumen");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Consec", SqlDbType.SmallInt, 0, "Consec");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Tesis", SqlDbType.NVarChar, 0, "Tesis");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Sala", SqlDbType.TinyInt, 0, "Sala");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Epoca", SqlDbType.TinyInt, 0, "Epoca");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Fuente", SqlDbType.TinyInt, 0, "Fuente");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Pagina", SqlDbType.NVarChar, 0, "Pagina");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IdProd", SqlDbType.Int, 0, "IdProd");

                    dataAdapter.Update(dataSet, "Ejecutoria");

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                }
                dataReader.Close();

            }
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            finally
            {
                connectionCT9BD2.Close();
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
            SqlConnection conneCT9BD2 = (SqlConnection)Conexion.GetConecctionManttoCE();
            SqlCommand cmd;
            SqlDataReader dataReader;

            List<DocumentoTO> Ejecutorias = new List<DocumentoTO>();

            try
            {
                conneCT9BD2.Open();
                string sqlCadena = "SELECT Id,Rubro,Asunto,Promovente FROM Ejecutoria WHERE idProd = " + idTipoEje + " ORDER BY ConsecIndx";

                cmd = new SqlCommand(sqlCadena, conneCT9BD2);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    DocumentoTO ejecutoria = new DocumentoTO();
                    ejecutoria.Id = Convert.ToInt32(dataReader["Id"].ToString());
                    ejecutoria.Rubro = dataReader["Rubro"].ToString();
                    ejecutoria.Asunto = dataReader["Asunto"].ToString();
                    ejecutoria.Promovente = dataReader["Promovente"].ToString();

                    Ejecutorias.Add(ejecutoria);
                }
            }
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                conneCT9BD2.Close();
            }

            return Ejecutorias;
        }

        public void DeleteDocumento(long ius, int idTipoEje)
        {
            SqlConnection conneCT9BD2 = (SqlConnection)Conexion.GetConecctionManttoCE();
            SqlCommand cmd;

            cmd = conneCT9BD2.CreateCommand();
            cmd.Connection = conneCT9BD2;

            try
            {
                conneCT9BD2.Open();

                cmd.CommandText = "DELETE FROM Ejecutoria WHERE Id = " + ius + " AND idProd = " + idTipoEje;
                cmd.ExecuteNonQuery();

            }
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            finally
            {
                conneCT9BD2.Close();
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
                    ejecutoria.Id = Convert.ToInt32(dataReader["Id"].ToString());
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
                    string miQry = "SELECT * FROM Ejecutoria Where Id = " + ejecutoriaDTO.Id;
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
                        dr["Id"] = ejecutoriaDTO.Id;
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
                        dr["IdProd"] = 2;

                        dataSet.Tables["Ejecutoria"].Rows.Add(dr);

                        dataAdapter.InsertCommand = connectionCT9BD2.CreateCommand();
                        dataAdapter.InsertCommand.CommandText = "INSERT INTO Ejecutoria(Id,Rubro,ConsecIndx,LocExp,LocAbr,Parte,Asunto,Promovente,API,MI,LI," +
                            "Volumen,Consec,Tesis,Sala,Epoca,Fuente,Pagina,IdProd)" +
                            " VALUES(@Id,@Rubro,@ConsecIndx,@LocExp,@LocAbr,@Parte,@Asunto,@Promovente,@API,@MI,@LI," +
                            "@Volumen,@Consec,@Tesis,@Sala,@Epoca,@Fuente,@Pagina,@IdProd)";

                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Rubro", SqlDbType.NText, 0, "Rubro");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@ConsecIndx", SqlDbType.Int, 0, "ConsecIndx");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@locExp", SqlDbType.NText, 0, "LocExp");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@LocAbr", SqlDbType.NVarChar, 0, "LocAbr");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Parte", SqlDbType.TinyInt, 0, "Parte");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Asunto", SqlDbType.NVarChar, 0, "Asunto");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Promovente", SqlDbType.NText, 0, "Promovente");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@API", SqlDbType.NText, 0, "API");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@MI", SqlDbType.NText, 0, "MI");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@LI", SqlDbType.NText, 0, "LI");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Volumen", SqlDbType.SmallInt, 0, "Volumen");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Consec", SqlDbType.SmallInt, 0, "Consec");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Tesis", SqlDbType.NVarChar, 0, "Tesis");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Sala", SqlDbType.TinyInt, 0, "Sala");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Epoca", SqlDbType.TinyInt, 0, "Epoca");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Fuente", SqlDbType.TinyInt, 0, "Fuente");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Pagina", SqlDbType.NVarChar, 0, "Pagina");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IdProd", SqlDbType.Int, 0, "IdProd");

                        dataAdapter.Update(dataSet, "Ejecutoria");

                        dataSet.Dispose();
                        dataAdapter.Dispose();

                        setPartesEjecutorias(getPartesEjecutoria(ejecutoriaDTO.Id));

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
                    dr["Id"] = ejecutoriaDTO.Id;
                    dr["Consec"] = ejecutoriaDTO.Consec;
                    dr["txtParte"] = ejecutoriaDTO.TextoParte;
                    dr["TI"] = ejecutoriaDTO.TextoIndx;
                    dr["TontaUnica"] = ejecutoriaDTO.TontaUnica;
                    dr["Parte"] = ejecutoriaDTO.Parte;
                    dr["ConsecIndx"] = ejecutoriaDTO.ConsecIndx;

                    dataSet.Tables["Ejecutoria"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connectionEpsSQL.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO ParteEjecutoria(Id,Consec,txtParte,TI,TontaUnica,Parte,ConsecIndx)" +
                        " VALUES(@Id,@Consec,@txtParte,@TI,@TontaUnica,@Parte,@ConsecIndx)";

                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Consec", SqlDbType.Int, 0, "Consec");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@txtParte", SqlDbType.NText, 0, "txtParte");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TI", SqlDbType.NText, 0, "TI");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TontaUnica", SqlDbType.SmallInt, 0, "TontaUnica");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Parte", SqlDbType.TinyInt, 0, "Parte");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@ConsecIndx", SqlDbType.Int, 0, "ConsecIndx");

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
