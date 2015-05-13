using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ManttoProductosAlternos.Controller;
using ManttoProductosAlternos.Dto;
using ManttoProductosAlternos.Model;
using ManttoProductosAlternos.Reportes;
using ManttoProductosAlternos.Singletons;
using ManttoProductosAlternos.Utils;
using ScjnUtilities;
using Telerik.Windows.Controls;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for AgrMantto.xaml
    /// </summary>
    public partial class AgrMantto : Window
    {
        // private int idSeleccionado = 0;
        //List<TreeViewItem> arbolAgraria = new List<TreeViewItem>();
        //private Temas temaSeleccionado = null;
        //private TesisDTO tesisSeleccionada = null;
        //public int IdProducto = 0;
        //private bool expande = true;
        //private int find = 0;
        //private List<string> busqueda = new List<string>();
        //List<TesisDTO> tesisRelacionadas = null;
        //TreeViewItem nodoSelect = null;

        ObservableCollection<Temas> arbolTemas;// = new List<TreeViewItem>();

        AgrManttoController controller;

        public AgrMantto(ObservableCollection<Temas> arbolTemas)
        {
            InitializeComponent();
            this.arbolTemas = arbolTemas;
            controller = new AgrManttoController(this,this.arbolTemas);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AccesoModel model = new AccesoModel();
            model.ObtenerPermisos();

            
            controller.SetEnableThemes();
            controller.WindowLoad(1);
        }
        
        private void TvAgrariaSelectedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            controller.CambioTemaSeleccionado();
        }

        private void BtnAgregarClick(object sender, RoutedEventArgs e)
        {
            controller.AgregarRelacion(txtIUS.Text);
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
            e.Handled = StringUtilities.IsTextAllowed(e.Text); 
        }

        private void BtnGeneraArbolClick(object sender, RoutedEventArgs e)
        {
            //AgrRelacionesMes mes = new AgrRelacionesMes(tvAgraria);
            //mes.GeneraWord();
        }

        private void BtnSalirClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        

        private void BtnIr_Click(object sender, RoutedEventArgs e)
        {
            controller.MoveGridToIus((Convert.ToInt32(txtNumIUSBuscr.Text)));
        }

        

        private void RadRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            RadRibbonButton boton = sender as RadRibbonButton;

            switch (boton.Name)
            {
                case "BtnCopiar":
                    controller.CopiarRelaciones();
                    break;
                case "BtnCortar":
                    controller.CortarRelaciones();
                    break;
                case "BtnPegar":
                    controller.PegarRelaciones();
                    break;
                case "BtnDelOne":
                    controller.DeleteOne();
                    break;
                case "BtnDelAll":
                    
                    controller.DeleteAll();
                    break;
                case "BtnBuscar":
                    controller.BuscarDuplicados();
                    break;
                case "BtnAddTema":
                    controller.AgregarTema();
                    break;
                case "BtnUpdTema":
                    controller.ActualizaTema();
                    break;
                case "BtnDelTema":
                    controller.EliminaTema();
                    break;
                case "BtnListadoTesis": controller.ShowListaTesis();
                    break;
                case "BtnOrdenar": controller.OrdenaTesis();
                    break;
            }
        }

       

        private void ButtonMaterias_Click(object sender, RoutedEventArgs e)
        {
            RadRibbonButton boton = sender as RadRibbonButton;
            controller.WindowLoad(Convert.ToInt16(boton.Uid));

            //switch (boton.Name)
            //{
            //    case "RBtnAgraria":
            //        controller.WindowLoad(1);
            //        break;
            //    case "RBtnSuspension":
            //        controller.WindowLoad(2);
            //        break;
            //    case "RBtnImprocedencia":
            //        controller.WindowLoad(3);
            //        break;
            //    case "RBtnScjn":
            //        controller.WindowLoad(4);
            //        break;
            //    case "RBtnElectoral":
            //        controller.WindowLoad(15);
            //        break;
            //    case "RBtnPermisos":
            //        break;
            //}
        }

        private void DgTesisSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            controller.RegistroActivado(sender);
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            controller.Searcher(((TextBox)sender).Text);
        }

        
    }
}