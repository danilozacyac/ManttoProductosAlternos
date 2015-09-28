using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ManttoProductosAlternos.DBAccess;
using ManttoProductosAlternos.Dto;
using ManttoProductosAlternos.Interface;
using ScjnUtilities;

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
            SqlConnection connection = Conexion.GetConnectionCt9bd3();
            SqlCommand cmd;
            SqlDataReader reader;

            DocumentoTO voto = null;

            try
            {
                connection.Open();
                string sqlCadena = "SELECT * FROM VotosParticulares WHERE Id = @ius";

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@ius", ius);
                reader = cmd.ExecuteReader();

                
                while (reader.Read())
                {
                    voto = new DocumentoTO();
                    voto.Id = reader["Id"] as int? ?? -1;
                    voto.Rubro = reader["Rubro"].ToString();
                    voto.ConsecIndx = reader["ConsecIndx"] as int? ?? -1; 
                    voto.LocExp = reader["LocExp"].ToString();
                    voto.LocAbr = reader["LocAbr"].ToString();
                    voto.Parte = reader["Parte"] as int? ?? -1; 
                    voto.Asunto = reader["Asunto"].ToString();
                    voto.Promovente = reader["Promovente"].ToString();
                    voto.AsuntoIndx = reader["API"].ToString();
                    voto.DatosAsuntoIndx = reader["MI"].ToString();
                    voto.LocIndx = reader["LI"].ToString();
                    voto.Volumen = reader["Volumen"] as int? ?? -1; 
                    voto.Consec = 0;// Convert.ToInt32(dataReader["Consec"].ToString());
                    voto.Tesis = ""; reader["Tesis"].ToString();
                    voto.Sala = 0;//Convert.ToInt32(dataReader["Sala"].ToString());
                    voto.Epoca = 0;// Convert.ToInt32(dataReader["Epoca"].ToString());
                    voto.Fuente = 0;// Convert.ToInt32(dataReader["Fuente"].ToString());
                    voto.Pagina = "";// dataReader["Pagina"].ToString();
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,VotosModel", "ManttoProductosAlternos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,VotosModel", "ManttoProductosAlternos");
            }
            finally
            {
                connection.Close();
            }
            return voto;
        }

        public void SetDocumento(DocumentoTO votoDto, int idTipoEje)
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
                string miQry = "SELECT * FROM VotosParticulares Where Id = " + votoDto.Id;
                cmd = new SqlCommand(miQry, connection);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();
                    string sqlCadena = "SELECT * FROM VotosParticulares WHERE id = 0";


                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                    dataAdapter.Fill(dataSet, "VotosParticulares");

                    dr = dataSet.Tables["VotosParticulares"].NewRow();
                    dr["Id"] = votoDto.Id;
                    dr["Rubro"] = votoDto.Rubro;
                    dr["ConsecIndx"] = votoDto.ConsecIndx;
                    dr["LocExp"] = votoDto.LocExp;
                    dr["LocAbr"] = votoDto.LocAbr;
                    dr["Parte"] = (votoDto.Parte == -1) ? 0 : votoDto.Parte;
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

                    dataAdapter.InsertCommand = connection.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO VotosParticulares(Id,Rubro,ConsecIndx,LocExp,LocAbr,Parte,Asunto,Promovente,API,MI,LI," +
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

                    dataAdapter.Update(dataSet, "VotosParticulares");

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                }
                reader.Close();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,VotosModel", "ManttoProductosAlternos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,VotosModel", "ManttoProductosAlternos");
            }
            finally
            {
                connection.Close();
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
            SqlConnection connection = Conexion.GetConecctionManttoCE();
            SqlCommand cmd;
            SqlDataReader reader;

            List<DocumentoTO> votos = new List<DocumentoTO>();

            try
            {
                connection.Open();
                string sqlCadena = "SELECT Id,Rubro,Asunto,Promovente FROM VotosParticulares WHERE idProd = " + idTipoEje + " ORDER BY ConsecIndx";

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DocumentoTO voto = new DocumentoTO();
                    voto.Id = Convert.ToInt32(reader["Id"].ToString());
                    voto.Rubro = reader["Rubro"].ToString();
                    voto.Asunto = reader["Asunto"].ToString();
                    voto.Promovente = reader["Promovente"].ToString();

                    votos.Add(voto);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,VotosModel", "ManttoProductosAlternos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,VotosModel", "ManttoProductosAlternos");
            }
            finally
            {
                connection.Close();
            }

            return votos;
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

                cmd.CommandText = "DELETE FROM VotosParticulares WHERE Id = " + ius + " AND idProd = " + idTipoEje;
                cmd.ExecuteNonQuery();
                
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,VotosModel", "ManttoProductosAlternos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,VotosModel", "ManttoProductosAlternos");
            }
            finally
            {
                connection.Close();
            }

        }

        
    }
}
