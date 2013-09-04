using System;
using System.Linq;
using System.Windows;
using ManttoProductosAlternos.Model;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String [] acceso;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frmAcceso frmAcceso = new frmAcceso();
            frmAcceso.ShowDialog();


            if (AccesoUsuarioModel.Llave == 0 || AccesoUsuarioModel.Llave == -1)
            {
                Close();
            }
            else
            {
                acceso = AccesoUsuarioModel.Programas.Split(',');

                /*
                 * 1  Agraria
                 * 2  Suspensión del acto reclamado
                 * 3  Improcedencia del Acto Reclamado
                 * 4  Facultades exclusivas de la SCJN
                 */
                if (acceso.Contains("1") || AccesoUsuarioModel.Grupo == 0)
                    btnAgraria.IsEnabled = true;
                if (acceso.Contains("2") || AccesoUsuarioModel.Grupo == 0)
                    btnSAR.IsEnabled = true;
                if (acceso.Contains("3") || AccesoUsuarioModel.Grupo == 0)
                    btnImprocedencia.IsEnabled = true;
                if (acceso.Contains("4") || AccesoUsuarioModel.Grupo == 0)
                    BtnSCJN.IsEnabled = true;
                if (acceso.Contains("15") || AccesoUsuarioModel.Grupo == 0)
                    BtnElectoral.IsEnabled = true;

                if (AccesoUsuarioModel.Grupo == 0)
                    btnAdmin.Visibility = Visibility.Visible;
            }
        }

        private void BtnAgraria_Click(object sender, RoutedEventArgs e)
        {
                int idProducto = 1;
                AgrMantto agr = new AgrMantto(idProducto);
                agr.ShowDialog();
        }

        private void BtnSar_Click(object sender, RoutedEventArgs e)
        {
            int idProducto = 2;
            AgrMantto agr = new AgrMantto(idProducto);
            agr.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            frmPermisos per = new frmPermisos();
            per.ShowDialog();
        }

        private void BtnImprocedencia_Click(object sender, RoutedEventArgs e)
        {
            int idProducto = 3;
            AgrMantto agr = new AgrMantto(idProducto);
            agr.ShowDialog();
        }

        private void BtnScjn_Click(object sender, RoutedEventArgs e)
        {/*
            */
            SelDocument selecciona = new SelDocument();
            selecciona.ShowDialog();
        }

        private void BtnElectoral_Click(object sender, RoutedEventArgs e)
        {
            int idProducto = 15;
            AgrMantto agr = new AgrMantto(idProducto);
            agr.ShowDialog();
        }

       
    }
}
