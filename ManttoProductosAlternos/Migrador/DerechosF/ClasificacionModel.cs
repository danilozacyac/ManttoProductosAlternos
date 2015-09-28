using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using ScjnUtilities;

namespace ManttoProductosAlternos.Migrador.DerechosF
{
    public class ClasificacionModel
    {

        private List<Relaciones> relaciones;
        private List<Clasificacion> temas;

        public List<Clasificacion> GetTemas()
        {
            temas = new List<Clasificacion>();

            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["BaseDH"].ToString());
            OleDbCommand cmd;
            OleDbDataReader reader;

            string sqlCadena = "SELECT * FROM TemasBusqueda_luis";

            try
            {
                connection.Open();
                cmd = new OleDbCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Clasificacion tema = new Clasificacion();

                    tema.IdClasifDisco = Convert.ToInt32(reader["Id"]);
                    tema.IdClasifScjn = Convert.ToInt32(reader["IdScjn"]);
                    tema.IdClasifPc = Convert.ToInt32(reader["IdPC"]);
                    tema.IdClasifTcc = Convert.ToInt32(reader["IdTCC"]);
                    tema.Descripcion = reader["descrip"].ToString();

                    temas.Add(tema);

                    Console.WriteLine(tema.Descripcion);
                }
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionModel", "ChecaPrecedentes");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionModel", "ChecaPrecedentes");
            }
            finally
            {
                connection.Close();
            }
            return temas;
        }


        /// <summary>
        /// Obtiene las relaciones que se establecieron en el Apéndice 2011
        /// </summary>
        /// <returns></returns>
        public void GetRelacionesCongelado()
        {

            if (relaciones == null)
                relaciones = new List<Relaciones>();

            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["BaseCongelado"].ToString());
            OleDbCommand cmd;
            OleDbDataReader reader;

            string sqlCadena = "SELECT * FROM TemasIus";

            try
            {
                connection.Open();
                cmd = new OleDbCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Relaciones relacion = new Relaciones()
                    {
                        IdClasifDisco = reader["Id"] as int? ?? 0,
                        Ius = reader["Ius"] as int? ?? 0
                    };

                    
                    relaciones.Add(relacion);

                }
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionModel", "ChecaPrecedentes");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionModel", "ChecaPrecedentes");
            }
            finally
            {
                connection.Close();
            }

        }


        /// <summary>
        /// Obtiene las relaciones que se establecieron para las tesis posteriores a la 
        /// publicación del apéndice
        /// </summary>
        public List<Relaciones> GetRelacionesPostApendice()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BaseIUS"].ToString());
            SqlCommand cmd;
            SqlDataReader reader;

            try
            {
                connection.Open();
                foreach (Clasificacion tema in temas)
                {

                    string sqlCadena = "SELECT M.IUS,M.IdMatSGA FROM Tesis_MatSGA M INNER JOIN Tesis T " +
                                       " ON T.IUS = M.IUS WHERE T.[ta_tj] = 1 and Parte <> 99 AND IdMatSGA = " +
                                       tema.IdClasifScjn + " OR IdMatSGA = " + tema.IdClasifTcc + " OR IdMatSGA = " + tema.IdClasifPc;

                    cmd = new SqlCommand(sqlCadena, connection);
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Relaciones relacion = new Relaciones()
                            {
                                IdClasifDisco = tema.IdClasifDisco,
                                Ius = reader["Ius"] as int? ?? 0
                            };


                            relaciones.Add(relacion);
                        }
                    }

                    reader.Close();
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionModel", "ChecaPrecedentes");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionModel", "ChecaPrecedentes");
            }
            finally
            {
                connection.Close();
            }

            return relaciones;
        }


        public void SetRelaciones(BackgroundWorker worker)
        {
            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["BaseDH"].ToString());
            OleDbCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            int currentProgress = 1;

            try
            {
                connection.Open();

                foreach (Relaciones relacion in relaciones)
                {
                    cmd.CommandText = "INSERT INTO TemasIUS VALUES(" + relacion.IdClasifDisco + "," + relacion.Ius + ",0)";
                    cmd.ExecuteNonQuery();

                    worker.ReportProgress(currentProgress);
                    currentProgress++;
                }
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionModel", "ChecaPrecedentes");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionModel", "ChecaPrecedentes");
            }
            finally
            {
                connection.Close();
            }

        }


        /// <summary>
        /// Elimina todas las relaciones existentes
        /// </summary>
        public void EliminaRelaciones()
        {
            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["BaseDH"].ToString());
            OleDbCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();
                cmd.CommandText = "DELETE FROM TemasIUS";
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionModel", "ChecaPrecedentes");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionModel", "ChecaPrecedentes");
            }
            finally
            {
                connection.Close();
            }

        }




        #region Congelado Apendice
        private List<Relaciones> relacionesCongelado;

        /// <summary>
        /// Obtiene la clasificación que se hizo de las tesis para el Apéndice de 2011, 
        /// dichas relaciones se obtienen de la base de datos Apendic6 de la columna 
        /// sección
        /// </summary>
        public void GeneraCongelado()
        {
            relacionesCongelado = new List<Relaciones>();

            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["BaseCongelado"].ToString()) ;
            OleDbDataReader reader;
            OleDbCommand cmd;

            foreach (Clasificacion tema in temas)
            {

                Console.WriteLine(tema.Descripcion);
                /*
                 * Primero traemos las relaciones establecidas en el apéndice
                 * */

                string sqlCadena = "SELECT ius4,Seccion FROM Tesis WHERE ";

                if (tema.IdClasifDisco == 40)
                {
                    sqlCadena += "seccion = " + tema.IdClasifScjn + " OR seccion = 12020240";
                }
                else if (tema.IdClasifDisco == 100)
                {
                    sqlCadena += "seccion = " + tema.IdClasifScjn + " OR seccion = 12020100";
                }
                else if (tema.IdClasifDisco == 265)
                {
                    sqlCadena += "seccion = 12010265 OR seccion = " + tema.IdClasifTcc;
                }
                else
                {
                    sqlCadena += "seccion = " + tema.IdClasifScjn + " OR seccion = " + tema.IdClasifTcc;
                }

                cmd = new OleDbCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        relacionesCongelado.Add(new Relaciones()
                            {
                                IdClasifDisco = tema.IdClasifDisco,
                                Ius = reader["ius4"] as int? ?? 0
                            });
                    }

                reader.Close();
            }
        }



        public void GetTotalTesis()
        {
            temas = new List<Clasificacion>();

            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["BaseDH"].ToString());
            OleDbCommand cmd;
            OleDbDataReader reader;
            connection.Open();
            foreach (Clasificacion tema in temas)
            {

                string sqlCadena = "SELECT COUNT(IUS) AS total FROM TemasBusqueda_luis WHERE id= @id ";

                try
                {

                    cmd = new OleDbCommand(sqlCadena, connection);
                    cmd.Parameters.AddWithValue("@id", tema.IdClasifDisco);
                    reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        tema.TotalTesisRelacionadas = Convert.ToInt32(reader["total"]);
                    }
                }
                catch (OleDbException ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionModel", "ChecaPrecedentes");
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionModel", "ChecaPrecedentes");
                }
                finally
                {
                    connection.Close();
                }
            }

        }


        #endregion

    }
}
