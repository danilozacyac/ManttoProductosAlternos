﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScjnUtilities;

namespace ManttoProductosAlternos.Migrador.DerechosF
{
    public class ClasificacionModel
    {

        private List<Relaciones> relaciones;
        private List<Clasificacion> temas;

        public void GetTemas()
        {
            temas = new List<Clasificacion>();

            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["BaseDH"].ToString());
            OleDbCommand cmd;
            OleDbDataReader reader;

            string sqlCadena = "SELECT * FROM TemasBusqueda_luis";

            try
            {

                cmd = new OleDbCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Clasificacion tema = new Clasificacion();

                    tema.IdClasifDisco = reader["Id"] as int? ?? 0;
                    tema.IdClasifScjn = reader["IdScjn"] as int? ?? 0;
                    tema.IdClasifPc = reader["IdPC"] as int? ?? 0;
                    tema.IdClasifTcc = reader["IdTCC"] as int? ?? 0;
                    tema.Descripcion = reader["descrip"].ToString();

                    temas.Add(tema);

                    Console.WriteLine(tema.Descripcion);
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

            return temas;
        }


        /// <summary>
        /// Obtiene las relaciones que se establecieron en el Apéndice 2011
        /// </summary>
        /// <returns></returns>
        public void GetRelacionesCongelado()
        {
            List<Clasificacion> clasificacion = new List<Clasificacion>();

            if (relaciones == null)
                relaciones = new List<Relaciones>();

            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["BaseCongelado"].ToString());
            OleDbCommand cmd;
            OleDbDataReader reader;

            string sqlCadena = "SELECT * FROM TemasIus";

            try
            {

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



        pu



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




        #endregion

    }
}