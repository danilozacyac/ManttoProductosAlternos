using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ManttoProductosAlternos.Dto;
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
        ObservableCollection<Temas> arbolTemas;

        public LaunchWindow()
        {
            InitializeComponent();
            worker.DoWork += this.WorkerDoWork;
            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
                arbolTemas = TemasSingletons.Temas(1);
            else
                arbolTemas = TemasSingletons.Temas(Convert.ToInt16(acceso[0]));
        }

        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.BusyIndicator.IsBusy = false;

            AgrMantto mainW = new AgrMantto(arbolTemas);
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
