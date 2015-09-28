using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using ManttoProductosAlternos.DBAccess;
using ManttoProductosAlternos.Dto;
using ScjnUtilities;

namespace ManttoProductosAlternos.Model
{
    public class BitacoraModel
    {

        /// <summary>
        /// Ingresa una entrada a la bitácora con los movimientos que se generan en la información
        /// </summary>
        /// <param name="temaModificado">Tema que se esta modificando o que se esta creando</param>
        /// <param name="tipoModificacion">El tipo de modificación que se esta realizando</param>
        /// <param name="edoAnterior">Estado anterior de la información</param>
        public void SetBitacoraEntry(Temas temaModificado, int tipoModificacion,string edoAnterior)
        {
            SqlConnection connection = Conexion.GetConecctionManttoCE();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Bitacora WHERE IdProd = 1000";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Temas");

                dr = dataSet.Tables["Temas"].NewRow();
                dr["IdTema"] = temaModificado.IdTema;
                dr["TipoModif"] = tipoModificacion;
                dr["EdoAnterior"] = edoAnterior;
                dr["Usuario"] = Environment.MachineName;
                dr["IdProd"] = temaModificado.IdProducto;

                dataSet.Tables["Temas"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();

                dataAdapter.InsertCommand.CommandText = "INSERT INTO Bitacora(IdTema,TipoModif,EdoAnterior,Usuario,IdProd)" +
                                      "values(@IdTema,@TipoModif,@EdoAnterior,@Usuario,@IdProd)";
                    
                dataAdapter.InsertCommand.Parameters.Add("@IdTema", SqlDbType.Int, 0, "IdTema");
                dataAdapter.InsertCommand.Parameters.Add("@TipoModif", SqlDbType.Int, 0, "TipoModif");
                dataAdapter.InsertCommand.Parameters.Add("@EdoAnterior", SqlDbType.VarChar, 0, "EdoAnterior");
                dataAdapter.InsertCommand.Parameters.Add("@Usuario", SqlDbType.VarChar, 0, "Usuario");
                dataAdapter.InsertCommand.Parameters.Add("@IdProd", SqlDbType.Int, 0, "IdProd");

                dataAdapter.Update(dataSet, "Temas");
                dataSet.Dispose();
                dataAdapter.Dispose();


            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,BitacoraModel", "ManttoProductosAlternos");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,BitacoraModel", "ManttoProductosAlternos");
            }
            finally
            {
                connection.Close();
            }
        
        }

    }
}
