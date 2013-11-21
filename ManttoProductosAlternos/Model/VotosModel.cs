using System;
using System.Collections.Generic;
using ManttoProductosAlternos.DTO;
using System.Data.SqlClient;
using System.Windows;
using ManttoProductosAlternos.DBAccess;
using System.Data;
using System.Data.Common;
using ManttoProductosAlternos.Interface;

namespace ManttoProductosAlternos.Model
{
    public class VotosModel : IDocumentos
    {
        /// <summary>
        /// Trae la información de los votos a partir de su número de IUS
        /// </summary>
        /// <param name="ius"></param>
        /// <returns></returns>
        public DocumentoTO GetDocumentoPorIus(long ius)
        {
            SqlConnection conneDSQL = (SqlConnection)Conexion.GetConecctionDsql();
            SqlCommand cmd;
            SqlDataReader dataReader;

            DocumentoTO voto = null;

            try
            {
                conneDSQL.Open();
                string sqlCadena = "SELECT * FROM VotosParticulares WHERE Id = " + ius;

                cmd = new SqlCommand(sqlCadena, conneDSQL);
                dataReader = cmd.ExecuteReader();

                
                while (dataReader.Read())
                {
                    voto = new DocumentoTO();
                    voto.Id = Convert.ToInt32(dataReader["Id"].ToString());
                    voto.Rubro = dataReader["Rubro"].ToString();
                    voto.ConsecIndx = Convert.ToInt32(dataReader["ConsecIndx"].ToString());
                    voto.LocExp = dataReader["LocExp"].ToString();
                    voto.LocAbr = dataReader["LocAbr"].ToString();
                    voto.Parte = Convert.ToInt32(dataReader["Parte"].ToString());
                    voto.Asunto = dataReader["Asunto"].ToString();
                    voto.Promovente = dataReader["Promovente"].ToString();
                    voto.AsuntoIndx = dataReader["API"].ToString();
                    voto.DatosAsuntoIndx = dataReader["MI"].ToString();
                    voto.LocIndx = dataReader["LI"].ToString();
                    voto.Volumen = Convert.ToInt32(dataReader["Volumen"].ToString());
                    voto.Consec = 0;// Convert.ToInt32(dataReader["Consec"].ToString());
                    voto.Tesis = ""; dataReader["Tesis"].ToString();
                    voto.Sala = 0;//Convert.ToInt32(dataReader["Sala"].ToString());
                    voto.Epoca = 0;// Convert.ToInt32(dataReader["Epoca"].ToString());
                    voto.Fuente = 0;// Convert.ToInt32(dataReader["Fuente"].ToString());
                    voto.Pagina = "";// dataReader["Pagina"].ToString();
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
            return voto;
        }

        public void SetDocumento(DocumentoTO votoDto, int idTipoEje)
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
                string miQry = "SELECT * FROM VotosParticulares Where Id = " + votoDto.Id;
                cmd = new SqlCommand(miQry, connectionCT9BD2);
                dataReader = cmd.ExecuteReader();

                if (!dataReader.HasRows)
                {
                    dataReader.Close();
                    string sqlCadena = "SELECT * FROM VotosParticulares WHERE id = 0";


                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionCT9BD2);

                    dataAdapter.Fill(dataSet, "VotosParticulares");

                    dr = dataSet.Tables["VotosParticulares"].NewRow();
                    dr["Id"] = votoDto.Id;
                    dr["Rubro"] = votoDto.Rubro;
                    dr["ConsecIndx"] = votoDto.ConsecIndx;
                    dr["LocExp"] = votoDto.LocExp;
                    dr["LocAbr"] = votoDto.LocAbr;
                    dr["Parte"] = votoDto.Parte;
                    dr["Asunto"] = votoDto.Asunto;
                    dr["Promovente"] = votoDto.Promovente;
                    dr["API"] = votoDto.AsuntoIndx;
                    dr["MI"] = votoDto.DatosAsuntoIndx;
                    dr["LI"] = votoDto.LocIndx;
                    dr["Volumen"] = votoDto.Volumen;
                    dr["Consec"] = DBNull.Value;
                    dr["tesis"] = DBNull.Value;
                    dr["Sala"] = DBNull.Value;
                    dr["Epoca"] = DBNull.Value;
                    dr["Fuente"] = DBNull.Value;
                    dr["Pagina"] = DBNull.Value;
                    dr["IdProd"] = idTipoEje;

                    dataSet.Tables["VotosParticulares"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connectionCT9BD2.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO VotosParticulares(Id,Rubro,ConsecIndx,LocExp,LocAbr,Parte,Asunto,Promovente,API,MI,LI," +
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

                    dataAdapter.Update(dataSet, "VotosParticulares");

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
        /// Obtiene los votos que fueron relacionadas con alguno de los apartados de Facultades
        /// de la Suprema Corte 
        /// </summary>
        /// <param name="ius"></param>
        /// <returns></returns>
        public List<DocumentoTO> GetDocumentosRelacionados(int idTipoEje)
        {
            SqlConnection conneCT9BD2 = (SqlConnection)Conexion.GetConecctionManttoCE();
            SqlCommand cmd;
            SqlDataReader dataReader;

            List<DocumentoTO> Votos = new List<DocumentoTO>();

            try
            {
                conneCT9BD2.Open();
                string sqlCadena = "SELECT Id,Rubro,Asunto,Promovente FROM VotosParticulares WHERE idProd = " + idTipoEje + " ORDER BY ConsecIndx";

                cmd = new SqlCommand(sqlCadena, conneCT9BD2);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    DocumentoTO voto = new DocumentoTO();
                    voto.Id = Convert.ToInt32(dataReader["Id"].ToString());
                    voto.Rubro = dataReader["Rubro"].ToString();
                    voto.Asunto = dataReader["Asunto"].ToString();
                    voto.Promovente = dataReader["Promovente"].ToString();

                    Votos.Add(voto);
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

            return Votos;
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

                cmd.CommandText = "DELETE FROM VotosParticulares WHERE Id = " + ius + " AND idProd = " + idTipoEje;
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

        /// <summary>
        /// Obtiene el o los votos relacionados con una Tesis a partir de su número de IUS
        /// </summary>
        /// <param name="ius"></param>
        /// <returns>Lista de Ejecutorias</returns>
        public List<VotoDTO> getVotosRelacionadas(long ius)
        {
            SqlConnection conneDSQL = (SqlConnection)Conexion.GetConecctionDsql();
            SqlCommand cmd;
            SqlDataReader dataReader;

            List<VotoDTO> Votos = new List<VotoDTO>();

            try
            {
                conneDSQL.Open();
                string sqlCadena = "SELECT E.* FROM VotosParticulares E INNER JOIN Eje_IUS  EI  ON EI.ID = E.ID WHERE Tpo = 3 AND EI.IUS = " + ius;

                cmd = new SqlCommand(sqlCadena, conneDSQL);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    VotoDTO voto = new VotoDTO();
                    voto.IdTema = Convert.ToInt32(dataReader["IdTema"].ToString());
                    voto.Rubro = dataReader["Rubro"].ToString();
                    voto.ConsecIndx = Convert.ToInt32(dataReader["ConsecIndx"].ToString());
                    voto.LocExp = dataReader["LocExp"].ToString();
                    voto.LocAbr = dataReader["LocAbr"].ToString();
                    voto.Parte = Convert.ToInt32(dataReader["Parte"].ToString());
                    voto.Asunto = dataReader["Asunto"].ToString();
                    voto.Promovente = dataReader["Promovente"].ToString();
                    voto.AsuntoIndx = dataReader["API"].ToString();
                    voto.DatosAsuntoIndx = dataReader["MI"].ToString();
                    voto.LocIndx = dataReader["LI"].ToString();
                    voto.Volumen = Convert.ToInt32(dataReader["Volumen"].ToString());

                    Votos.Add(voto);
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

            return Votos;
        }

        public List<VotoDTO> getPartesVoto(long ius)
        {
            SqlConnection conneDSQL = (SqlConnection)Conexion.GetConecctionDsql();
            SqlCommand cmd;
            SqlDataReader dataReader;

            List<VotoDTO> Votos = new List<VotoDTO>();

            try
            {
                conneDSQL.Open();
                string sqlCadena = "SELECT * FROM ParteVotos WHERE ID = " + ius;

                cmd = new SqlCommand(sqlCadena, conneDSQL);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    VotoDTO ejecutoria = new VotoDTO();
                    ejecutoria.Id = Convert.ToInt32(dataReader["IdTema"].ToString());
                    ejecutoria.Consec = Convert.ToInt32(dataReader["Consec"].ToString());
                    ejecutoria.TextoParte = dataReader["txtParte"].ToString();
                    ejecutoria.TextoIndx = dataReader["TI"].ToString();
                    ejecutoria.TontaUnica = Convert.ToInt32(dataReader["TontaUnica"].ToString());
                    ejecutoria.Parte = Convert.ToInt32(dataReader["Parte"].ToString());
                    ejecutoria.ConsecIndx = Convert.ToInt32(dataReader["ConsecIndx"].ToString());

                    Votos.Add(ejecutoria);
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

            return Votos;
        }

        public void setVotosRelacionados(List<VotoDTO> Votos, long ius)
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
                foreach (VotoDTO votosDTO in Votos)
                {
                    string miQry = "SELECT * FROM VotosParticulares Where Id = " + votosDTO.Id;
                    cmd = new SqlCommand(miQry, connectionCT9BD2);
                    dataReader = cmd.ExecuteReader();

                    if (!dataReader.HasRows)
                    {
                        dataReader.Close();
                        string sqlCadena = "SELECT * FROM VotosParticulares WHERE id = 0";

                        dataAdapter = new SqlDataAdapter();
                        dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionCT9BD2);

                        dataAdapter.Fill(dataSet, "Votos");

                        dr = dataSet.Tables["Votos"].NewRow();
                        dr["Id"] = votosDTO.Id;
                        dr["Rubro"] = votosDTO.Rubro;
                        dr["ConsecIndx"] = votosDTO.ConsecIndx;
                        dr["LocExp"] = votosDTO.LocExp;
                        dr["LocAbr"] = votosDTO.LocAbr;
                        dr["Parte"] = votosDTO.Parte;
                        dr["Asunto"] = votosDTO.Asunto;
                        dr["Promovente"] = votosDTO.Promovente;
                        dr["API"] = votosDTO.AsuntoIndx;
                        dr["MI"] = votosDTO.DatosAsuntoIndx;
                        dr["LI"] = votosDTO.LocIndx;
                        dr["Volumen"] = votosDTO.Volumen;
                        dr["Consec"] = DBNull.Value;
                        dr["tesis"] = DBNull.Value;
                        dr["Sala"] = DBNull.Value;
                        dr["Epoca"] = DBNull.Value;
                        dr["Fuente"] = DBNull.Value;
                        dr["Pagina"] = DBNull.Value;
                        dr["IdProducto"] = 2;

                        dataSet.Tables["Votos"].Rows.Add(dr);

                        dataAdapter.InsertCommand = connectionCT9BD2.CreateCommand();
                        dataAdapter.InsertCommand.CommandText = "INSERT INTO VotosParticulares(Id,Rubro,ConsecIndx,LocExp,LocAbr,Parte,Asunto,Promovente,API,MI,LI," +
                            "Volumen,Consec,Tesis,Sala,Epoca,Fuente,Pagina,IdProducto)" +
                            " VALUES(@Id,@Rubro,@ConsecIndx,@LocExp,@LocAbr,@Parte,@Asunto,@Promovente,@API,@MI,@LI," +
                            "@Volumen,@Consec,@Tesis,@Sala,@Epoca,@Fuente,@Pagina,@IdProducto)";

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
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IdProducto", SqlDbType.Int, 0, "IdProducto");

                        dataAdapter.Update(dataSet, "Votos");

                        dataSet.Dispose();
                        dataAdapter.Dispose();

                        setPartesVotos(getPartesVoto(votosDTO.Id));

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

        public void setPartesVotos(List<VotoDTO> Votos)
        {
            SqlConnection connectionCT9BD2 = new SqlConnection();
            DbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;


            string sqlCadena = "SELECT * FROM ParteVotos WHERE id = 0";

            connectionCT9BD2 = (SqlConnection)Conexion.GetConecctionManttoCE();

            try
            {
                foreach (VotoDTO votoDTO in Votos)
                {
                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionCT9BD2);

                    dataAdapter.Fill(dataSet, "Votos");

                    dr = dataSet.Tables["Votos"].NewRow();
                    dr["Id"] = votoDTO.Id;
                    dr["Consec"] = votoDTO.Consec;
                    dr["txtParte"] = votoDTO.TextoParte;
                    dr["TI"] = votoDTO.TextoIndx;
                    dr["TontaUnica"] = votoDTO.TontaUnica;
                    dr["Parte"] = votoDTO.Parte;
                    dr["ConsecIndx"] = votoDTO.ConsecIndx;

                    dataSet.Tables["Votos"].Rows.Add(dr);

                    dataAdapter.InsertCommand = connectionCT9BD2.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO ParteVotos(Id,Consec,txtParte,TI,TontaUnica,Parte,ConsecIndx)" +
                        " VALUES(@Id,@Consec,@txtParte,@TI,@TontaUnica,@Parte,@ConsecIndx)";

                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Consec", SqlDbType.TinyInt, 0, "Consec");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@txtParte", SqlDbType.NText, 0, "txtParte");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TI", SqlDbType.NText, 0, "TI");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TontaUnica", SqlDbType.Int, 0, "TontaUnica");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Parte", SqlDbType.TinyInt, 0, "Parte");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@ConsecIndx", SqlDbType.Int, 0, "ConsecIndx");

                    dataAdapter.Update(dataSet, "Votos");

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
                connectionCT9BD2.Close();
            }
        }
         * */
    }
}
