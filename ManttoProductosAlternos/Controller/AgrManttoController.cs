using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ManttoProductosAlternos.Dto;
using ManttoProductosAlternos.Model;
using ManttoProductosAlternos.Utils;
using ScjnUtilities;
using Telerik.Windows.Controls;

namespace ManttoProductosAlternos.Controller
{
    public class AgrManttoController
    {
        ObservableCollection<Temas> arbolTemas; //= new List<TreeViewItem>();
        private Temas temaSeleccionado = null;
        private TesisDTO tesisSeleccionada = null;
        private int idProducto = 0;
        private bool expande = true;
        private int find = 0;
        private List<string> busqueda = new List<string>();
        List<TesisDTO> tesisRelacionadas = null;
        TreeViewItem nodoSelect = null;

        private UpdateProgressBarDelegate updatePbDelegate = null;

        private delegate void UpdateProgressBarDelegate(
            System.Windows.DependencyProperty dp, Object value);

        private readonly AgrMantto main;

        public AgrManttoController(AgrMantto main, ObservableCollection<Temas> arbolTemas)
        {
            this.main = main;
            this.arbolTemas = arbolTemas;
        }

        public void WindowLoad(int idProducto)
        {
            this.idProducto = idProducto;
            main.Ribbon.ApplicationName = VarGlobales.TituloVentanas(idProducto);

            main.tvAgraria.Items.Clear();

            GeneraArbol arbol = new GeneraArbol();
            //arbolTemas = arbol.GeneraAgraria(0, idProducto);

            //foreach (TreeViewItem tema in arbolTemas)
            //{
            //    main.tvAgraria.Items.Add(tema);
            //}

            main.tvAgraria.DataContext = arbolTemas;

            bool enable = idProducto == 4 ? false : true;

            main.BuscadorTxt.IsEnabled = enable;
            main.BtnAddTema.IsEnabled = enable;
            main.BtnUpdTema.IsEnabled = enable;
            main.BtnDelTema.IsEnabled = enable;
        }

        private String[] acceso;

        public void SetEnableThemes()
        {
            updatePbDelegate =
                new UpdateProgressBarDelegate(main.pbBusqueda.SetValue);

            acceso = AccesoUsuarioModel.Programas.Split(',');

            /*
             * 1  Agraria
             * 2  Suspensión del acto reclamado
             * 3  Improcedencia del Acto Reclamado
             * 4  Facultades exclusivas de la SCJN
             * 15 Electoral
             */
            if (acceso.Contains("1") || AccesoUsuarioModel.Grupo == 0)
                main.RBtnAgraria.IsEnabled = true;
            if (acceso.Contains("2") || AccesoUsuarioModel.Grupo == 0)
                main.RBtnSuspension.IsEnabled = true;
            if (acceso.Contains("3") || AccesoUsuarioModel.Grupo == 0)
                main.RBtnImprocedencia.IsEnabled = true;
            if (acceso.Contains("4") || AccesoUsuarioModel.Grupo == 0)
                main.RBtnScjn.IsEnabled = true;
            if (acceso.Contains("15") || AccesoUsuarioModel.Grupo == 0)
                main.RBtnElectoral.IsEnabled = true;
            if (AccesoUsuarioModel.Grupo == 0)
            {
                main.RBtnPermisos.Visibility = Visibility.Visible;
                main.RBtnPermisos.IsEnabled = true;
            }
        }

        public void CambioTemaSeleccionado()
        {
            if (main.tvAgraria.SelectedItem != null)
            {
                //nodoSelect = (TreeViewItem)main.tvAgraria.SelectedItem;
                temaSeleccionado = main.tvAgraria.SelectedItem as Temas;

                tesisRelacionadas = new TesisModel(idProducto).GetTesisRelacionadas(temaSeleccionado.IdTema);
                tesisRelacionadas = tesisRelacionadas.Distinct().ToList();
                main.dgTesis.DataContext = tesisRelacionadas;

                main.lblTemaSeleccionado.Text = temaSeleccionado.Tema;
                main.txtRegistros.Text = tesisRelacionadas.Count + " Registros";
            }
        }

        public void AgregarRelacion(string registroIus)
        {
            if (registroIus.Length < 6 || registroIus.Length > 7)
            {
                MessageBox.Show("Introduzca un número IUS Válido");
                return;
            }

            if (temaSeleccionado != null)
            {
                bool existeRelacion = false;
                foreach (TesisDTO tesis in tesisRelacionadas)
                {
                    if (tesis.Ius == Convert.ToInt32(registroIus))
                    {
                        existeRelacion = true;
                        break;
                    }
                }

                if (!existeRelacion)
                {
                    TesisModel tesisModel = new TesisModel(idProducto);
                    tesisModel.InsertaTesis(Convert.ToInt32(registroIus), temaSeleccionado.IdTema);
                    CambioTemaSeleccionado();
                }
                else
                {
                    MessageBox.Show("El tema ya fue relacionado con esta tesis");
                }
            }
            else
            {
                MessageBox.Show("Seleccione el tema con el cual relacionara la tesis");
            }

            if (idProducto != 1)
                main.txtIUS.Text = "";
        }

        #region Busqueda

        public void Searcher(string textoBuscado)
        {
            if (String.IsNullOrEmpty(textoBuscado) || String.IsNullOrWhiteSpace(textoBuscado))
            {
                main.Cursor = Cursors.Wait;
                expande = false;
                Buscador(main.tvAgraria, busqueda, 0);
                main.Cursor = Cursors.Arrow;
            }
            else
            {
                busqueda.Clear();

                main.Cursor = Cursors.Wait;
                if (textoBuscado.Length > 3)
                {
                    if (busqueda.Count > 0)
                    {
                        Searcher(String.Empty);
                        busqueda.Clear();
                    }

                    find = 0;
                    expande = true;
                    foreach (string cadena in textoBuscado.TrimEnd(' ').TrimStart(' ').Split(' '))
                    {
                        busqueda.Add(cadena);
                    }

                    Buscador(main.tvAgraria, busqueda, 1);
                }

                main.Cursor = Cursors.Arrow;

                if (find == 0)
                    MessageBox.Show("No existe el texto buscado");
            }
        }

        private void Buscador(RadTreeView treeView, List<string> llave, int foregroundColor)
        {
            find = 0;
            main.pbBusqueda.Visibility = Visibility.Visible;
            main.pbBusqueda.Minimum = 0;
            main.pbBusqueda.Maximum = main.tvAgraria.Items.Count;
            main.pbBusqueda.Value = 0;

            foreach (Temas nItem in treeView.Items)
            {
                main.pbBusqueda.Value += 1;
                int cont = 0;
                foreach (string bus in llave)
                {
                    if (StringUtilities.QuitaCarCad(nItem.Tema.ToString().ToUpper()).Contains(StringUtilities.QuitaCarCad(bus.ToUpper())))
                    {
                        cont++;

                        if (find == 0)
                        {
                            nItem.IsExpanded = expande;
                            
                            nItem.IsSelected = true;
                            //nItem.BringIntoView();
                        }
                    }

                    if (cont == llave.Count)
                    {
                        nItem.Foreground = foregroundColor;
                        find++;
                    }
                }
                BuscaItem(nItem, llave, foregroundColor); 

                Dispatcher.CurrentDispatcher.Invoke(updatePbDelegate,
                    System.Windows.Threading.DispatcherPriority.Background,
                    new object[] { ProgressBar.ValueProperty, main.pbBusqueda.Value });
            }
            main.pbBusqueda.Visibility = Visibility.Hidden;
        }

        // Busca recursivamente en todos los nodos hasta encontrar el patron
        private void BuscaItem(Temas node, List<string> llave, int foregroundColor)
        {
            foreach (Temas child in node.SubTemas)
            {
                //TreeViewItem child = childx as TreeViewItem;
                int cont = 0;
                foreach (string bus in llave)
                {
                    if (StringUtilities.QuitaCarCad(child.Tema.ToString().ToUpper()).Contains(StringUtilities.QuitaCarCad(bus.ToUpper())))
                    {
                        cont++;
                        if (find == 0)
                        {
                            child.IsSelected = true;
                            //child.BringIntoView();
                        }
                        // return; cuando se pone return solo tomo el primer nodo encontrado por cada hijo
                    }

                    if (cont == llave.Count)
                    {
                        child.IsExpanded = expande;
                        child.Foreground = foregroundColor;
                        //child.Focus();
                        //child.IsSelected = true;
                        //AbreRaiz(child);
                        find++;
                    }

                    BuscaItem(child, llave, foregroundColor);
                }
            }
        }

        // Este metodo simplemente expande los nodos padre con un tipo de backtraking.
        //private void AbreRaiz(object root)
        //{
        //    if (root.GetType() == typeof(TreeViewItem))
        //    {
        //        TreeViewItem node = root as TreeViewItem;

        //        if (root.GetType() == typeof(TreeViewItem))
        //        {
        //            if (((TreeViewItem)root).Parent.GetType() == typeof(TreeViewItem))
        //            {
        //                ((TreeViewItem)node.Parent).IsExpanded = expande;

        //                AbreRaiz((TreeViewItem)node.Parent);
        //            }
        //        }
        //    }
        //}

        #endregion

        #region Grid

        public void RegistroActivado(object sender)
        {
            tesisSeleccionada = ((RadGridView)sender).SelectedItem as TesisDTO;
        }

        public void MoveGridToIus(long nIus)
        {
            //int nRow = 0;
            bool find = false;

            foreach (TesisDTO item in main.dgTesis.Items)
            {
                if (nIus == item.Ius)
                {
                    //item.IsSelected = true;
                    //nRow = item.Index;
                    find = true;
                    //main.dgTesis.ActiveRecord = item;
                    main.dgTesis.CurrentItem = item;
                    main.dgTesis.SelectedItem = item;
                    main.dgTesis.ScrollIntoView(item);
                    break;
                }
            }

            if (!find)
                MessageBox.Show("Número de registro no encontrado");
        }

        #endregion

        #region Metodos Temas

        public void AgregarTema()
        {
            if (idProducto == 1)
            {
                if (temaSeleccionado != null)
                {
                    VarGlobales.temaNuevo = null;
                    AgrAgregaTema agr = new AgrAgregaTema(temaSeleccionado.IdTema, temaSeleccionado.Nivel, idProducto);
                    agr.ShowDialog();

                    if (VarGlobales.temaNuevo != null)
                    {
                        TreeViewItem treeNode = new TreeViewItem();
                        treeNode.Tag = VarGlobales.temaNuevo;
                        treeNode.Header = VarGlobales.temaNuevo.Tema;

                        if (VarGlobales.temaNuevo.Nivel == 0)
                        {
                            main.tvAgraria.Items.Add(treeNode);
                        }
                        else
                        {
                            nodoSelect.Items.Add(treeNode);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione la voz que contendrá  el tema que desea agregar");
                }
            }
            else
            {
                VarGlobales.temaNuevo = null;
                AgrAgregaTema agr = new AgrAgregaTema(0, 0, idProducto);
                agr.ShowDialog();

                if (VarGlobales.temaNuevo != null)
                {
                    TreeViewItem treeNode = new TreeViewItem();
                    treeNode.Tag = VarGlobales.temaNuevo;
                    treeNode.Header = VarGlobales.temaNuevo.Tema;

                    if (VarGlobales.temaNuevo.Nivel == 0)
                    {
                        main.tvAgraria.Items.Add(treeNode);
                    }
                    else
                    {
                        nodoSelect.Items.Add(treeNode);
                    }
                }
            }
        }

        public void ActualizaTema()
        {
            if (temaSeleccionado != null)
            {
                AgrAgregaTema agr = new AgrAgregaTema(0, 0, temaSeleccionado, idProducto);
                agr.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione el tema que desea actualizar", "Atención : ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void EliminaTema()
        {
            if (temaSeleccionado != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Estas seguro de eliminar el tema " + temaSeleccionado.Tema + " y todas sus tesis relacionadas?", "Error Interno", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    TemasModel temasModel = new TemasModel(idProducto);
                    temasModel.EliminaTema(temaSeleccionado.IdTema);
                    main.tvAgraria.Items.Remove(nodoSelect);
                }
            }
            else
            {
                MessageBox.Show("Seleccione el tema que desea eliminar", "Atención : ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Metodos Relaciones

        public Temas temaCopia;

        public void CopiarRelaciones()
        {
            TreeViewItem item = main.tvAgraria.SelectedItem as TreeViewItem;
            temaCopia = item.Tag as Temas;
            temaCortar = null;
        }

        public Temas temaCortar;

        public void CortarRelaciones()
        {
            TreeViewItem item = main.tvAgraria.SelectedItem as TreeViewItem;
            temaCortar = item.Tag as Temas;
            temaCopia = null;
        }

        public void PegarRelaciones()
        {
            MessageBoxResult result;
            TesisModel model = new TesisModel(idProducto);

            if (temaCopia != null)
            {
                result = MessageBox.Show("¿Estas segur@ que deseas copiar las tesis del tema \"" + temaCopia.Tema + "\" al tema \"" +
                                         temaSeleccionado.Tema + "\"?", "ATENCIÓN:", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    model.CopiaTesis(temaCopia.IdTema, temaSeleccionado.IdTema);
                }
            }
            else if (temaCortar != null)
            {
                result = MessageBox.Show("¿Estas segur@ que deseas eliminart todas las tesis del tema \"" + temaCortar.Tema + "\" y pegarlas al tema \"" +
                                         temaSeleccionado.Tema + "\"?", "ATENCIÓN:", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    model.CortarTesis(temaCortar.IdTema, temaSeleccionado.IdTema);
                }
            }
            else
            {
                MessageBox.Show("Antes de pegar selecciona copiar/pegar mientras seleccionas el temas con las tesis de interes");
            }

            temaCopia = null;
            temaCortar = null;
        }

        public void DeleteOne()
        {
            if (tesisSeleccionada != null && temaSeleccionado != null)
            {
                TesisModel tesisModel = new TesisModel(idProducto);
                tesisModel.EliminaRelacion(tesisSeleccionada.Ius, temaSeleccionado.IdTema);
                CambioTemaSeleccionado();
            }
            else
            {
                MessageBox.Show("Seleccione la tesis cuya relación va a eliminar");
            }
        }

        public void DeleteAll()
        {
            MessageBoxResult result = MessageBox.Show("¿Estas segur@ que deseas eliminar todas las tesis relacionadas al tema \"" +
                                                      temaSeleccionado.Tema + "\" ?", "ATENCIÓN:", MessageBoxButton.YesNo, MessageBoxImage.Question);
            TesisModel model = new TesisModel(idProducto);

            if (result == MessageBoxResult.Yes)
            {
                model.EliminaTesis(temaSeleccionado.IdTema);
            }
        }

        public void BuscarDuplicados()
        {
            TemasModel model = new TemasModel();

            string str = ConfigurationManager.AppSettings.Get("RutaTxtErrorFile");
            List<Temas> temas = model.GetTemasForReview(idProducto);

            ObservableCollection<List<Temas>> repetidas = new ObservableCollection<List<Temas>>();

            foreach (Temas tema in temas)
            {
                model.SearchForDuplicates(repetidas, tema.TemaStr, idProducto);
            }

            foreach (List<Temas> lista in repetidas)
            {
                foreach (Temas tmstr in lista)
                {
                    System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(str, true);
                    try
                    {
                        streamWriter.WriteLine(" ");
                        streamWriter.WriteLine(" ");
                        streamWriter.WriteLine(" ");

                        streamWriter.WriteLine(string.Concat("*********************", DateTime.Now.ToString(), "***************************"));
                        streamWriter.WriteLine(string.Concat(tmstr.IdTema + "      " + tmstr.Tema, " "));

                        streamWriter.WriteLine("***************************************************************************************");
                    }
                    finally
                    {
                        if (streamWriter != null)
                        {
                            ((System.IDisposable)streamWriter).Dispose();
                        }
                    }
                }
            }
            Process.Start(str);
        }

        #endregion

        #region Otros Ribbon

        public void ShowListaTesis()
        {
            frmListaTesis tesis = new frmListaTesis(idProducto);
            tesis.ShowDialog();
        }

        public void OrdenaTesis()
        {
            TesisModel tesisModel = new TesisModel(idProducto);
            tesisModel.SetConsecIndx();
        }
        
        #endregion
    }
}