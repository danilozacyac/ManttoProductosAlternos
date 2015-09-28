using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ManttoProductosAlternos.Dto;
using ManttoProductosAlternos.Migrador.DerechosF;
using Telerik.Windows.Controls;

namespace ManttoProductosAlternos.Migrador
{
    /// <summary>
    /// Interaction logic for MigrationWin.xaml
    /// </summary>
    public partial class MigrationWin
    {
        RadProgressBar myProgressBar;
        System.Windows.Controls.Label myLabel;

        public MigrationWin()
        {
            InitializeComponent();

            worker.DoWork += this.WorkerDoWork;
            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
            worker.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanged);
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDh_Click(object sender, RoutedEventArgs e)
        {
            
        }

        #region Background Worker

        private BackgroundWorker worker = new BackgroundWorker();
        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            //Suspension del acto reclamado
            this.CurrentProduct(2, new RadProgressBar[] { SusTes, SusTem, SusRel }, new System.Windows.Controls.Label[] { LSusTes, LSusTem, LSusRel });

            //Improcedencia del juicio de amparo
            this.CurrentProduct(3, new RadProgressBar[] { ImpTes, ImpTem, ImpRel }, new System.Windows.Controls.Label[] { LImpTes, LImpTem, LImpRel });

            //Facultades exclusivas de la SCJN
            this.CurrentProduct(4, new RadProgressBar[] { FacTes }, new System.Windows.Controls.Label[] { LFacTes });

            //Derechos Fundamentales
            this.CurrentProduct(10, new RadProgressBar[] { DhRel }, new System.Windows.Controls.Label[] { LDhRel });

            //Electoral
            this.CurrentProduct(15, new RadProgressBar[] { ElTes, ElTem, ElRel }, new System.Windows.Controls.Label[] { LElTes, LElTem, LElRel });

            
        }

        void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // The progress percentage is a property of e
            myProgressBar.Value = e.ProgressPercentage;
        }

        private void RBtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.worker.CancelAsync();
            //this.RBtnCancelar.IsEnabled = false;
        }

        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //BusyIndicator.IsBusy = false;
            MessageBox.Show("Migración finalizada");

        }

        //private void LaunchBusyIndicator()
        //{
        //    if (!worker.IsBusy)
        //    {
        //        BusyIndicator.IsBusy = true;
        //        worker.RunWorkerAsync();

        //    }
        //}

        void CurrentProduct(int idProducto, RadProgressBar[] myBars, System.Windows.Controls.Label [] myLabels)
        {
            MigradorModel model = new MigradorModel(idProducto);

            

            if (idProducto != 4 && idProducto != 10)
            {
                model.EliminaRegistros();
                myProgressBar = myBars[0];
                myLabel = myLabels[0];
                List<int> tesisRelacionadas = model.GetTesisRelacionadasByProducto();
                this.Dispatcher.BeginInvoke((Action)(() => UpdateContentLabel(tesisRelacionadas.Count.ToString())));
                this.Dispatcher.BeginInvoke((Action)(() => UpdateMaximun(tesisRelacionadas.Count)));
                model.InsertaIuses(tesisRelacionadas, worker);


                myProgressBar = myBars[1];
                myLabel = myLabels[1];
                List<Temas> temas = model.GetTemas();
                this.Dispatcher.BeginInvoke((Action)(() => UpdateContentLabel(temas.Count.ToString())));
                this.Dispatcher.BeginInvoke((Action)(() => UpdateMaximun(temas.Count)));
                model.InsertaTemas(temas, worker);

                myProgressBar = myBars[2];
                myLabel = myLabels[2];
                List<Temas> relaciones = model.GetRelaciones();
                this.Dispatcher.BeginInvoke((Action)(() => UpdateContentLabel(relaciones.Count.ToString())));
                this.Dispatcher.BeginInvoke((Action)(() => UpdateMaximun(relaciones.Count)));
                model.InsertaTemasIus(relaciones, worker);
            }
            else if(idProducto == 4)
            {
                model.EliminaRegistros();
                myProgressBar = myBars[0];
                myLabel = myLabels[0];
                List<TesisDTO> tesisRelacionadas = model.GetTesisRelacionadasScjn();
                this.Dispatcher.BeginInvoke((Action)(() => UpdateContentLabel(tesisRelacionadas.Count.ToString())));
                this.Dispatcher.BeginInvoke((Action)(() => UpdateMaximun(tesisRelacionadas.Count)));
                model.InsertaTemasIusScjn(tesisRelacionadas, worker);
            }
            else if (idProducto == 10)
            {
                ClasificacionModel myModel = new ClasificacionModel();
                myModel.EliminaRelaciones();
                myModel.GetTemas();

                myProgressBar = myBars[0];
                myLabel = myLabels[0];
                myModel.GetRelacionesCongelado();
                List<Relaciones> relaciones = myModel.GetRelacionesPostApendice();
                this.Dispatcher.BeginInvoke((Action)(() => UpdateContentLabel(relaciones.Count.ToString())));
                this.Dispatcher.BeginInvoke((Action)(() => UpdateMaximun(relaciones.Count)));
                myModel.SetRelaciones(worker);
            }
        }


        void UpdateContentLabel(string labelContent)
        {
            myLabel.Content = labelContent;
        }

        private void BtnComenzar_Click(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
        }

        //void UpdateListContent()
        //{
        //    LstRegs.DataContext = listaTesisNoConcuerdan;
        //}

        void UpdateMaximun(int maxValue)
        {
            myProgressBar.Maximum = maxValue;
        }

        #endregion
    }
}
