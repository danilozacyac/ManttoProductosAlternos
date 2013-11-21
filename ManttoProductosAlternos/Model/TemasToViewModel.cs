using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ManttoProductosAlternos.DTO;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Data.Common;
using System.Configuration;
using UtilsAlternos;

namespace ManttoProductosAlternos.Model
{
    public class TemaToViewModel
    {
        public ObservableCollection<Temas> TemasArbol { get; set; }

        private readonly string textoBuscado;

        private static ObservableCollection<Temas> tematico;

        private List<int> temasEnLista = new List<int>();

        public static ObservableCollection<Temas> Tematico
        {
            get
            {
                if (tematico == null)
                    tematico = new TemaToViewModel().GetTemas(null,0);

                return tematico;
            }
        }

        public TemaToViewModel() { }

        public TemaToViewModel(int idProducto)
        {
            TemasArbol = this.GetTemas(null,idProducto);
        }

        public TemaToViewModel(String textoBuscado)
        {
            this.textoBuscado = textoBuscado;
            TemasArbol = this.GetTemasBusqueda(null);
        }




        public ObservableCollection<Temas> GetTemas(Temas parentModule, int idProducto)
        {
            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            ObservableCollection<Temas> modulos = new ObservableCollection<Temas>();

            try
            {
                sqlConne.Open();

                string sqlCadena = "SELECT * " +
                                   "FROM Temas WHERE Padre = @Padre AND IdProd = @IdProd ORDER BY TemaStr ";
                SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                SqlParameter name = cmd.Parameters.Add("@Padre", SqlDbType.Int, 0);
                name.Value = (parentModule != null) ? parentModule.IdTema : 0;
                SqlParameter materia = cmd.Parameters.Add("@IdProd", SqlDbType.Int, 0);
                materia.Value = idProducto;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Temas tema;

                        if (parentModule != null)
                            tema = new Temas(parentModule, true);
                        else
                            tema = new Temas(null, true);

                        tema.IdProducto = Convert.ToInt32(reader["IdProd"]);
                        tema.IdTema = Convert.ToInt32(reader["id"]);
                        tema.Padre = Convert.ToInt32(reader["Padre"]);
                        tema.Nivel = Convert.ToInt32(reader["Nivel"]);
                        tema.Tema = reader["Tema"].ToString();
                        tema.TemaStr = reader["TemaStr"].ToString();
                        tema.Orden = Convert.ToInt32(reader["Orden"]);
                        tema.LInicial = Convert.ToChar(reader["LetraInicial"].ToString());

                        modulos.Add(tema);
                    }
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
                sqlConne.Close();
            }
            return modulos;
        }

        public ObservableCollection<Temas> GetTemasBusqueda(Temas parentModule)
        {
            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            ObservableCollection<Temas> modulos = new ObservableCollection<Temas>();

            
                try
                {
                    sqlConne.Open();

                    string sqlCadena = "SELECT * " +
                                       "FROM Temas WHERE (" + this.ArmaCadenaBusqueda(textoBuscado) + ")  AND IdProd = @IdProd  ORDER BY TemaStr ";
                    SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                    SqlParameter materia = cmd.Parameters.Add("@IdProd", SqlDbType.Int, 0);
                    materia.Value = parentModule.IdProducto;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Temas tema = new Temas(null, true);

                            tema.IdProducto = Convert.ToInt32(reader["IdProd"]);
                            tema.IdTema = Convert.ToInt32(reader["id"]);
                            tema.Padre = Convert.ToInt32(reader["Padre"]);
                            tema.Nivel = Convert.ToInt32(reader["Nivel"]);
                            tema.Tema = reader["Tema"].ToString();
                            tema.TemaStr = reader["TemaStr"].ToString();
                            tema.Orden = Convert.ToInt32(reader["Orden"]);
                            tema.LInicial = Convert.ToChar(reader["LetraInicial"].ToString());

                            if (temasEnLista.Contains(tema.IdTema))
                            {
                            }
                            else
                            {
                                if (tema.Padre == 0)
                                {
                                    modulos.Add(tema);
                                    temasEnLista.Add(tema.IdTema);
                                }
                                else
                                {
                                    if (temasEnLista.Contains(tema.Padre))
                                    {
                                        foreach (Temas tematico in modulos)
                                        {
                                            if (tematico.IdTema == tema.Padre)
                                            {
                                                if (tematico.SubTemas == null)
                                                    tema.SubTemas = new ObservableCollection<Temas>();
                                                tema.Parent = tematico;

                                                tematico.SubTemas.Add(tema);
                                                temasEnLista.Add(tema.IdTema);
                                            }
                                            else
                                            {
                                                this.SearchParentNode(tema, tematico);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.GetSearchParents(tema, modulos);
                                    }
                                }
                            }
                        }
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
                    sqlConne.Close();
                }
            
            return SortSearch(this.SetParents(modulos));
        }

        //private ObservableCollection<Temas> GetTemasBusqueda(Temas parentModule, String sqlCadenaComplement)
        //{
        //    SqlConnection sqlConne = (SqlConnection)this.GetConnection();

        //    ObservableCollection<Temas> modulos = new ObservableCollection<Temas>();

            
        //        try
        //        {
        //            sqlConne.Open();

        //            string sqlCadena = "SELECT * " +
        //                               "FROM Temas WHERE (" + sqlCadenaComplement + ")  and idPadre <> -1 ORDER BY DescripcionStr ";
        //            SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
        //            SqlParameter materia = cmd.Parameters.Add("@IdMateria", SqlDbType.Int, 0);
        //            materia.Value = idMateria.Id;
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                {
        //                    TemaTO tema = new TemaTO(null, true);

        //                    tema.Materia = Convert.ToInt32(reader["Materia"]);
        //                    tema.IDTema = Convert.ToInt32(reader["idTema"]);
        //                    tema.IDPadre = Convert.ToInt32(reader["IDPadre"]);
        //                    tema.Nivel = Convert.ToInt32(reader["Nivel"]);
        //                    tema.Descripcion = reader["Descripcion"].ToString();
        //                    tema.IdOrigen = 99;
        //                    tema.TesisRelacionadas = Convert.ToInt32(reader["Total"]);

        //                    if (temasEnLista.Contains(tema.IDTema))
        //                    {
        //                    }
        //                    else
        //                    {
        //                        if (tema.IDPadre == 0)
        //                        {
        //                            modulos.Add(tema);
        //                            temasEnLista.Add(tema.IDTema);
        //                        }
        //                        else
        //                        {
        //                            if (temasEnLista.Contains(tema.IDPadre))
        //                            {
        //                                foreach (TemaTO tematico in modulos)
        //                                {
        //                                    if (tematico.IDTema == tema.IDPadre)
        //                                    {
        //                                        if (tematico.SubTemas == null)
        //                                            tema.SubTemas = new ObservableCollection<TemaTO>();
        //                                        tema.Parent = tematico;

        //                                        tematico.SubTemas.Add(tema);
        //                                        temasEnLista.Add(tema.IDTema);
        //                                    }
        //                                    else
        //                                    {
        //                                        this.SearchParentNode(tema, tematico);
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                this.GetSearchParents(tema, modulos);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch (SqlException sql)
        //        {
        //            MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
        //        }
        //        finally
        //        {
        //            sqlConne.Close();
        //        }
            
        //    return SortSearch(this.SetParents(modulos));
        //}

        //public ObservableCollection<Temas> GetTemasPorIus(Temas parentModule)
        //{
        //    SqlConnection sqlConne = (SqlConnection)this.GetConnection();

        //    ObservableCollection<Temas> modulos = new ObservableCollection<Temas>();

        //    String sqlIusMateria = "";

            
        //        try
        //        {
        //            sqlConne.Open();

        //            string sqlCadena = "SELECT idMateria,idTema FROM TemasTesis WHERE IUS = @IUS AND " +
        //                               " IdMateria = @IdMateria  ORDER BY IUS Asc";
        //            SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
        //            SqlParameter ius = cmd.Parameters.Add("@IUS", SqlDbType.Int, 0);
        //            ius.Value = textoBuscado;
        //            SqlParameter materia = cmd.Parameters.Add("@IdMateria", SqlDbType.Int, 0);
        //            materia.Value = idMateria.Id;
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                {
        //                    sqlIusMateria += " OR (idTema = " + reader["idTema"].ToString() + " AND Materia = " + reader["idMateria"].ToString() + ")";
        //                }
        //            }


        //        }
        //        catch (SqlException sql)
        //        {
        //            MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
        //        }
        //        finally
        //        {
        //            sqlConne.Close();
        //        }
            

        //    if (sqlIusMateria.Length > 2)
        //    {
        //        sqlIusMateria = sqlIusMateria.Substring(4);

        //        modulos = this.GetTemasBusqueda(null, sqlIusMateria);

        //        return SortSearch(this.SetParents(modulos));
        //    }
        //    else
        //    {
        //        MessageBox.Show("No existen temas relacionados al número de tesis ingresada");
        //        return new ObservableCollection<Temas>();
        //    }
        //}

        private ObservableCollection<Temas> SortSearch(ObservableCollection<Temas> moduloToSort)
        {
            List<Temas> temp = moduloToSort.ToList();

            temp.Sort((x, y) => string.Compare(x.TemaStr, y.TemaStr));

            foreach (Temas tema in temp)
            {
                this.SortSearch(tema.SubTemas);
            }

            ObservableCollection<Temas> result = new ObservableCollection<Temas>(temp);

            return result;
        }

        private void SearchParentNode(Temas temaHijo, Temas temaPadre)
        {
            foreach (Temas tema in temaPadre.SubTemas)
            {
                if (temaHijo.Padre == tema.IdTema)
                {
                    if (tema.SubTemas == null)
                        tema.SubTemas = new ObservableCollection<Temas>();
                    temaHijo.Parent = tema;
                    tema.SubTemas.Add(temaHijo);
                    temasEnLista.Add(temaHijo.IdTema);
                }
                else
                {
                    this.SearchParentNode(temaHijo, tema);
                }
            }
        }

        private void GetSearchParents(Temas temaHijo, ObservableCollection<Temas> modulos)
        {
            Temas temaPadre = this.GetTemaByIdTema(temaHijo.Padre, temaHijo.IdProducto);
            temaPadre.SubTemas = new ObservableCollection<Temas>();
            temaHijo.Parent = temaPadre;
            temaPadre.SubTemas.Add(temaHijo);

            temasEnLista.Add(temaHijo.IdTema);
            bool find = false;
            if (temasEnLista.Contains(temaPadre.Padre))
            {
                foreach (Temas tematico in modulos)
                {
                    if (tematico.IdTema == temaPadre.Padre)
                    {
                        if (tematico.SubTemas == null)
                            temaPadre.SubTemas = new ObservableCollection<Temas>();
                        temaPadre.Parent = tematico;
                        tematico.SubTemas.Add(temaPadre);
                        temasEnLista.Add(temaPadre.IdTema);

                        find = true;
                    }

                    if (!find)
                        GetRecursiveParents(temaPadre, tematico.SubTemas);
                }
            }
            else if (temaPadre.Padre == 0)
            {
                modulos.Add(temaPadre);
                temasEnLista.Add(temaPadre.IdTema);
            }
            else if (temaPadre.Padre == -1)
            {
                //Si tiene como padre menos uno no lo agrega porque quiere decir que fue eliminado
            }
            else
                this.GetSearchParents(temaPadre, modulos);
        }

        private void GetRecursiveParents(Temas tema, ObservableCollection<Temas> subtemas)
        {
            bool find = false;

            foreach (Temas tematico in subtemas)
            {
                if (tematico.IdTema == tema.Padre)
                {
                    if (tematico.SubTemas == null)
                        tema.SubTemas = new ObservableCollection<Temas>();
                    tema.Parent = tematico;
                    tematico.SubTemas.Add(tema);
                    temasEnLista.Add(tema.IdTema);

                    find = true;
                }

                if (!find)
                    GetRecursiveParents(tema, tematico.SubTemas);
            }
        }

        private ObservableCollection<Temas> SetParents(ObservableCollection<Temas> modulos)
        {
            foreach (Temas tema in modulos)
            {
                foreach (Temas subtema in tema.SubTemas)
                {
                    subtema.Parent = tema;
                    this.SetParents(subtema.SubTemas);
                }
            }
            return modulos;
        }

        private Temas GetTemaByIdTema(int idTema, int idProducto)
        {
            SqlConnection sqlConne = (SqlConnection)this.GetConnection();

            Temas tema = new Temas();
            try
            {
                sqlConne.Open();

                string sqlCadena = "SELECT * " +
                                   "FROM Temas WHERE  Id = @Id AND IdProd = @IdProd  ORDER BY TemaStr ";
                SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                SqlParameter idTemas = cmd.Parameters.Add("@Id", SqlDbType.Int, 0);
                idTemas.Value = idTema;
                SqlParameter materia = cmd.Parameters.Add("@IdProd", SqlDbType.Int, 0);
                materia.Value = idProducto;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        tema.IdProducto = Convert.ToInt32(reader["IdProd"]);
                        tema.IdTema = Convert.ToInt32(reader["id"]);
                        tema.Padre = Convert.ToInt32(reader["Padre"]);
                        tema.Nivel = Convert.ToInt32(reader["Nivel"]);
                        tema.Tema = reader["Tema"].ToString();
                        tema.TemaStr = reader["TemaStr"].ToString();
                        tema.Orden = Convert.ToInt32(reader["Orden"]);
                        tema.LInicial = Convert.ToChar(reader["LetraInicial"].ToString());


                    }
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
                sqlConne.Close();
            }
            return tema;
        }

        private String ArmaCadenaBusqueda(String textoBuscado)
        {
            String[] listadoPalabras = textoBuscado.Split(' ');

            String resultString1 = "'%";
            foreach (string palabra in listadoPalabras)
            {
                if (!Constants.STOPERS.Contains(palabra.Trim().ToLower()))
                    resultString1 += FlowDocumentHighlight.Normaliza(palabra.Trim()) + "%";
                //resultString += "OR DescripcionStr LIKE '%" + FlowDocumentHighlight.Normaliza( palabra.Trim() ) + "%' ";
            }
            resultString1 += "'";

            String resultString2 = "'%";
            foreach (string palabra in listadoPalabras.Reverse())
            {
                if (!Constants.STOPERS.Contains(palabra.Trim().ToLower()))
                    resultString2 += FlowDocumentHighlight.Normaliza(palabra.Trim()) + "%";
                //resultString += "OR DescripcionStr LIKE '%" + FlowDocumentHighlight.Normaliza( palabra.Trim() ) + "%' ";
            }
            resultString2 += "'";

            return "DescripcionStr LIKE " + resultString1 + " OR DescripcionStr LIKE " + resultString2;
        }

        private bool findParent = false;
        public void SearchParentAddSon(ObservableCollection<Temas> listaTemas, Temas temaHijo)
        {
            if (!findParent)
                foreach (Temas temaBuscado in listaTemas)
                {
                    if (temaBuscado.IdTema == temaHijo.Parent.IdTema)
                    {
                        temaBuscado.AddSubtema(temaHijo);
                        findParent = true;
                    }
                    else
                    {
                        this.SearchParentAddSon(temaBuscado.SubTemas, temaHijo);
                    }

                    if (findParent)
                        break;
                }
        }

        public void SearchParentDeleteSon(ObservableCollection<Temas> listaTemas, Temas temaHijo)
        {
            if (!findParent)
                foreach (Temas temaBuscado in listaTemas)
                {
                    if (temaBuscado.IdTema == temaHijo.Parent.IdTema)
                    {
                        temaBuscado.RemoveSubTema(temaHijo);
                        findParent = true;
                    }
                    else
                    {
                        this.SearchParentAddSon(temaBuscado.SubTemas, temaHijo);
                    }

                    if (findParent)
                        break;
                }
        }



        private DbConnection GetConnection()
        {
            String tipoAplicacion = ConfigurationManager.AppSettings.Get("tipoAplicacion");

            String bdStringSql;

            if (tipoAplicacion.Equals("PRUEBA"))
            {
                bdStringSql = ConfigurationManager.ConnectionStrings["TematicoPrueba"].ConnectionString;
            }
            else
                bdStringSql = ConfigurationManager.ConnectionStrings["Tematico"].ConnectionString;
            DbConnection realConnection = new SqlConnection(bdStringSql);
            return realConnection;


        }
    }
}
