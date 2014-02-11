using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ManttoProductosAlternos.Model;
using ManttoProductosAlternos.DTO;
using ManttoProductosAlternos.Utils;
using System.Text.RegularExpressions;
using ManttoProductosAlternos.Reportes;
using UtilsAlternos;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Editors;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for AgrMantto.xaml
    /// </summary>
    public partial class AgrMantto : Window
    {
        // private int idSeleccionado = 0;
        List<TreeViewItem> arbolAgraria = new List<TreeViewItem>();
        private Temas temaSeleccionado = null;
        private TesisDTO tesisSeleccionada = null;
        private int idProducto = 0;
        private bool expande = true;
        private int find = 0;
        private List<string> busqueda = new List<string>();
        List<TesisDTO> tesisRelacionadas = null;
        TreeViewItem nodoSelect = null;

        UpdateProgressBarDelegate updatePbDelegate = null;

        public AgrMantto(int idProducto)
        {
            InitializeComponent();
            this.idProducto = idProducto;
            updatePbDelegate =
                   new UpdateProgressBarDelegate(pbBusqueda.SetValue);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = MiscFunciones.TituloVentanas(idProducto);

            tvAgraria.Items.Clear();
            GeneraArbol arbol = new GeneraArbol();

            arbolAgraria = arbol.GeneraAgraria(0, idProducto);

            foreach (TreeViewItem tema in arbolAgraria)
            {
                tvAgraria.Items.Add(tema);
            }

            btnBuscar.IsEnabled = (idProducto == 4) ? false : true;
            btnRestableceer.IsEnabled = (idProducto == 4) ? false : true;
            txtBuscar.IsEnabled = (idProducto == 4) ? false : true;
            btnAgregarTema.IsEnabled = (idProducto == 4) ? false : true;
            btnActualizarTema.IsEnabled = (idProducto == 4) ? false : true;
            btnEliminarTema.IsEnabled = (idProducto == 4) ? false : true;

            dgTesis.FieldSettings.AllowEdit = false;
            dgTesis.FieldSettings.AllowResize = false;
            Style wrapstyle = new Style(typeof(XamTextEditor));
            wrapstyle.Setters.Add(new Setter(XamTextEditor.TextWrappingProperty, TextWrapping.Wrap));
            dgTesis.FieldSettings.EditorStyle = wrapstyle;
        }

        private void TvAgrariaSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tvAgraria.SelectedItem != null)
            {
                nodoSelect = (TreeViewItem)tvAgraria.SelectedItem;
                temaSeleccionado = (Temas)nodoSelect.Tag;

                tesisRelacionadas = new TesisModel(idProducto).GetTesisRelacionadas(temaSeleccionado.IdTema);
                tesisRelacionadas = tesisRelacionadas.Distinct().ToList();
                dgTesis.DataContext = tesisRelacionadas;

                lblTemaSeleccionado.Text = temaSeleccionado.Tema;
                txtRegistros.Text = tesisRelacionadas.Count + " Registros";

            }
        }

        private void BtnAgregarTemaClick(object sender, RoutedEventArgs e)
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
                            tvAgraria.Items.Add(treeNode);
                        }
                        else { nodoSelect.Items.Add(treeNode); }
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
                        tvAgraria.Items.Add(treeNode);
                    }
                    else { nodoSelect.Items.Add(treeNode); }
                }
            }
        }

        private void BtnActualizarTemaClick(object sender, RoutedEventArgs e)
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

        private void BtnEliminarTemaClick(object sender, RoutedEventArgs e)
        {
            if (temaSeleccionado != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Desea eliminar el tema " + temaSeleccionado.Tema + "?", "Error Interno", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    TemasModel temasModel = new TemasModel(idProducto);
                    temasModel.EliminaTema(temaSeleccionado.IdTema);
                    tvAgraria.Items.Remove(nodoSelect);
                }
            }
            else
            {
                MessageBox.Show("Seleccione el tema que desea eliminar", "Atención : ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }



        private void BtnEliminarClick(object sender, RoutedEventArgs e)
        {
            if (tesisSeleccionada != null && temaSeleccionado != null)
            {
                TesisModel tesisModel = new TesisModel(idProducto);
                tesisModel.EliminaRelacion(tesisSeleccionada.Ius, temaSeleccionado.IdTema);
                TvAgrariaSelectedItemChanged(sender, null);
            }
            else
            {
                MessageBox.Show("Seleccione la tesis cuya relación va a eliminar");
            }
        }

        private void BtnAgregarClick(object sender, RoutedEventArgs e)
        {
            if (txtIUS.Text.Length < 6 || txtIUS.Text.Length > 7)
            {
                MessageBox.Show("Introduzca un número IUS Válido");
                return;
            }

            if (temaSeleccionado != null)
            {
                bool existeRelacion = false;
                foreach (TesisDTO tesis in tesisRelacionadas)
                {
                    if (tesis.Ius == Convert.ToInt32(txtIUS.Text))
                    {
                        existeRelacion = true;
                        break;
                    }
                }

                if (!existeRelacion)
                {
                    TesisModel tesisModel = new TesisModel(idProducto);
                    tesisModel.InsertaTesis(Convert.ToInt32(txtIUS.Text), temaSeleccionado.IdTema);
                    TvAgrariaSelectedItemChanged(sender, null);
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
                txtIUS.Text = "";
        }

        private void TxtIusGotFocus(object sender, RoutedEventArgs e)
        {
            btnAgregar.IsDefault = true;
        }

        private void TxtIusLostFocus(object sender, RoutedEventArgs e)
        {
            btnAgregar.IsDefault = false;
        }

        private void TxtIusPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            // Regex NumEx = new Regex(@"^\d+(?:.\d{0,2})?$"); 
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text 
            return !regex.IsMatch(text);
        }

        private void BtnOrdenarClick(object sender, RoutedEventArgs e)
        {
            TesisModel tesisModel = new TesisModel(idProducto);
            tesisModel.SetConsecIndx();
        }

        private void BtnGeneraArbolClick(object sender, RoutedEventArgs e)
        {
            agrRelacionesMes mes = new agrRelacionesMes(tvAgraria);
            mes.GeneraWord();
        }

        private void BtnBuscarClick(object sender, RoutedEventArgs e)
        {
            busqueda.Clear();

            this.Cursor = Cursors.Wait;
            if (txtBuscar.Text.Length > 3)
            {
                if (busqueda.Count > 0)
                {
                    BtnRestableceerClick(sender, e);
                    busqueda.Clear();
                }

                find = 0;
                expande = true;
                foreach (string cadena in txtBuscar.Text.TrimEnd(' ').TrimStart(' ').Split(' '))
                {
                    busqueda.Add(cadena);
                }

                Buscador(tvAgraria, busqueda, Color.FromRgb(255, 0, 0));
            }
            ////////TematicoConst.miBusquedaPrin = busqueda;
            this.Cursor = Cursors.Arrow;


            if (find == 0)
                MessageBox.Show("No existe el texto buscado");
        }

        private void BtnRestableceerClick(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            expande = false;
            Buscador(tvAgraria, busqueda, Color.FromRgb(0, 0, 0));
            this.Cursor = Cursors.Arrow;
        }


        private void Buscador(System.Windows.Controls.TreeView treeView, List<string> llave, Color color)
        {
            find = 0;
            pbBusqueda.Visibility = Visibility.Visible;
            pbBusqueda.Minimum = 0;
            pbBusqueda.Maximum = tvAgraria.Items.Count;
            pbBusqueda.Value = 0;

            foreach (TreeViewItem nItem in treeView.Items)
            {
                pbBusqueda.Value += 1;
                int cont = 0;
                foreach (string bus in llave)
                {
                    if (FlowDocumentHighlight.QuitaCarCad(nItem.Header.ToString().ToUpper()).Contains(FlowDocumentHighlight.QuitaCarCad(bus.ToUpper())))
                    {
                        cont++;

                        if (find == 0)
                        {
                            try
                            {
                                nItem.IsSelected = true;
                            }
                            catch (NullReferenceException) { }

                            nItem.BringIntoView();
                        }
                    }

                    if (cont == llave.Count)
                    {
                        nItem.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                        find++;
                    }
                }
                BuscaItem(nItem, llave, color);

                Dispatcher.Invoke(updatePbDelegate,
                System.Windows.Threading.DispatcherPriority.Background,
                new object[] { ProgressBar.ValueProperty, pbBusqueda.Value });
            }
            pbBusqueda.Visibility = Visibility.Hidden;
        }

        // Busca recursivamente en todos los nodos hasta encontrar el patron
        private void BuscaItem(TreeViewItem node, List<string> llave, Color color)
        {
            foreach (object childx in node.Items)
            {
                TreeViewItem child = childx as TreeViewItem;

                int cont = 0;
                foreach (string bus in llave)
                {
                    if (FlowDocumentHighlight.QuitaCarCad(child.Header.ToString().ToUpper()).Contains(FlowDocumentHighlight.QuitaCarCad(bus.ToUpper())))
                    {
                        cont++;
                        if (find == 0)
                        {
                            //child.IsSelected = true;
                            child.BringIntoView();
                        }
                        // return; cuando se pone return solo tomo el primer nodo encontrado por cada hijo
                    }

                    if (cont == llave.Count)
                    {
                        child.IsExpanded = true;
                        child.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                        child.Focus();
                        //child.IsSelected = true;
                        AbreRaiz(child);
                        find++;
                    }

                    BuscaItem(child, llave, color);
                }
            }
        }

        // Este metodo simplemente expande los nodos padre con un tipo de backtraking.
        private void AbreRaiz(object root)
        {
            if (root.GetType() == typeof(TreeViewItem))
            {
                TreeViewItem node = root as TreeViewItem;

                if (root.GetType() == typeof(TreeViewItem))
                {
                    if (((TreeViewItem)root).Parent.GetType() == typeof(TreeViewItem))
                    {
                        ((
                        TreeViewItem)node.Parent).IsExpanded = expande;

                        AbreRaiz((
                        TreeViewItem)node.Parent);
                    }
                }
            }
        }


        private void BtnSalirClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DgTesisRecordActivated1(object sender, Infragistics.Windows.DataPresenter.Events.RecordActivatedEventArgs e)
        {
            if (e.Record is DataRecord)
            {
                // Cast the record passed in as a DataRecord
                DataRecord myRecord = (DataRecord)e.Record;
                tesisSeleccionada = (TesisDTO)myRecord.DataItem;
            }
        }

        private void BtnListaTesis_Click(object sender, RoutedEventArgs e)
        {
            frmListaTesis tesis = new frmListaTesis(idProducto);
            tesis.ShowDialog();
        }

        private delegate void UpdateProgressBarDelegate(
        System.Windows.DependencyProperty dp, Object value);

        private void BtnIr_Click(object sender, RoutedEventArgs e)
        {
                MoveGridToIus((Convert.ToInt32(txtNumIUSBuscr.Text)));
        }

        private void MoveGridToIus(long nIus)
        {
            //int nRow = 0;
            bool find = false;

            foreach (DataRecord item in dgTesis.Records)
            {
                if (nIus == Convert.ToInt32(item.Cells["Ius"].Value))
                {
                    item.IsSelected = true;
                    //nRow = item.Index;
                    find = true;
                    dgTesis.ActiveRecord = item;
                    break;
                }
            }

            if (!find)
                MessageBox.Show("Número de registro no encontrado");
        }
    }
}
