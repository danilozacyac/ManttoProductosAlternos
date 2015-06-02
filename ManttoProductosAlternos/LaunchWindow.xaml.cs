using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using ManttoProductosAlternos.Model;
using ManttoProductosAlternos.Singletons;
using Telerik.Windows.Controls;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Lógica de interacción para LaunchWindow.xaml
    /// </summary>
    public partial class LaunchWindow : Window
    {

        public LaunchWindow()
        {
            InitializeComponent();
            worker.DoWork += this.WorkerDoWork;
            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string tipoApp = ConfigurationManager.AppSettings["TipoAplicacion"].ToString();

            if (tipoApp.Equals("PRUEBA"))
                MessageBox.Show("Estas viendo datos de prueba, comunicate con tu administrador");


            AccesoModel model = new AccesoModel();
            model.ObtenerPermisos();

            if (AccesoUsuarioModel.Llave == 0 || AccesoUsuarioModel.Llave == -1)
            {
                MessageBox.Show("No tienes permiso para utilizar esta aplicación", "ERROR:", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {

                StyleManager.ApplicationTheme = new Windows8Theme();

                this.LaunchBusyIndicator();

                string path = ConfigurationManager.AppSettings["ErrorPath"].ToString();

                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }


        #region Background Worker

        private BackgroundWorker worker = new BackgroundWorker();
        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            String[] acceso = AccesoUsuarioModel.Programas.Split(',');


            if (AccesoUsuarioModel.Grupo == 0)
                TemasSingletons.Temas(1);
            else
                TemasSingletons.Temas(Convert.ToInt16(acceso[0]));
        }

        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.BusyIndicator.IsBusy = false;

            AgrMantto mainW = new AgrMantto();
            mainW.Show();

            this.Close();
        }

        private void LaunchBusyIndicator()
        {
            if (!worker.IsBusy)
            {
                this.BusyIndicator.IsBusy = true;
                worker.RunWorkerAsync();

            }
        }

        #endregion
    }
}
