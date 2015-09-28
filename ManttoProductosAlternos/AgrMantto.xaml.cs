using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ManttoProductosAlternos.Controller;
using ManttoProductosAlternos.Migrador;
using ManttoProductosAlternos.Migrador.DerechosF;
using ManttoProductosAlternos.Model;
using ScjnUtilities;
using Telerik.Windows.Controls;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for AgrMantto.xaml
    /// </summary>
    public partial class AgrMantto
    {
        AgrManttoController controller;

        public AgrMantto()
        {
            InitializeComponent();
            controller = new AgrManttoController(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AccesoModel model = new AccesoModel();
            model.ObtenerPermisos();

            String[] acceso = AccesoUsuarioModel.Programas.Split(',');
            
            controller.SetEnableThemes();

            if (AccesoUsuarioModel.Grupo == 0)
                controller.WindowLoad(1);
            else
                controller.WindowLoad(Convert.ToInt16(acceso[0]));

            this.ShowInTaskbar(this, "Mantenimiento de Productos Alternos");
        }

        public void ShowInTaskbar(RadWindow control, string title)
        {
            control.Show();
            var window = control.ParentOfType<Window>();
            window.ShowInTaskbar = true;
            window.Title = title;
            var uri = new Uri("pack://application:,,,/ManttoProductosAlternos;component/Resources/Books_128.png");
            window.Icon = BitmapFrame.Create(uri);
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

            int idElement = Convert.ToInt16(boton.Uid);

            if (idElement == 1000)
                controller.LaunchPermisos();
            else
                controller.WindowLoad(idElement);

        }

        private void DgTesisSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            controller.RegistroActivado(sender);
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            controller.Searcher(((TextBox)sender).Text);
        }

        private void BtnEjeVotos_Click(object sender, RoutedEventArgs e)
        {
            controller.VerEjecutoriasVotos();
        }

        private void BtnMigrador_Click(object sender, RoutedEventArgs e)
        {
            controller.LaunchMigrador();
        }

        
    }
}