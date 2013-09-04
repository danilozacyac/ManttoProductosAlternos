using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ManttoProductosAlternos.DTO;
using ManttoProductosAlternos.Model;
using Infragistics.Windows.Editors;
using Infragistics.Windows.DataPresenter;
using System.Text.RegularExpressions;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for frmListaTesis.xaml
    /// </summary>
    public partial class frmListaTesis : Window
    {
        private List<TesisDTO> tesisRelacionadas = null;
        private int idProducto;

        private long pIus = 0;
        private long pId;
        private string pTesis;
        private Int32 pRecordPos;

        public frmListaTesis(int idProducto)
        {
            InitializeComponent();
            this.idProducto = idProducto;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tesisRelacionadas = new TesisModel(idProducto).GetTesisPorTema();
            tesisRelacionadas = tesisRelacionadas.Distinct().ToList();
            dgTesis.DataContext = tesisRelacionadas;
            // xamDataGrid1.DataSource = tesisRelacionadas;

            /**
             * Buscar como realizar las acciones de las siguientes líneas desde el editor
             * */
            dgTesis.FieldSettings.AllowEdit = false;
            dgTesis.FieldSettings.AllowResize = false;
            Style wrapstyle = new Style(typeof(XamTextEditor));
            wrapstyle.Setters.Add(new Setter(XamTextEditor.TextWrappingProperty, TextWrapping.Wrap));
            dgTesis.FieldSettings.EditorStyle = wrapstyle;
        }

        private void btnIUS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MoveGridToIus((Convert.ToInt32(txtIUS.Text)));
            }
            catch (Exception)
            {
                MessageBox.Show("Introduzca un número de registro válido");
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (pIus != 0)
            {
                AlertDialog dialog = new AlertDialog(pIus, idProducto);
                dialog.ShowDialog();

                if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
                {
                    new TesisModel(idProducto).EliminaTesisLista(pIus);
                    dgTesis.Records[this.pRecordPos].Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void dgTesis_RecordActivated(object sender, Infragistics.Windows.DataPresenter.Events.RecordActivatedEventArgs e)
        {
            if (e.Record is DataRecord)
            {
                DataRecord myRecord = (DataRecord)e.Record;
                this.pIus = Convert.ToInt32(myRecord.Cells[0].Value);
                //this.pIus = Convert.ToInt32(myRecord.Cells[1].Value);
                //this.pTesis = myRecord.Cells[2].Value.ToString();
                this.pRecordPos = myRecord.Index;

            }
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

        private void dgTesis_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject source = e.OriginalSource as DependencyObject;
            if (source == null)
                return;

            DataRecordPresenter drp = Infragistics.Windows.Utilities.GetAncestorFromType(source,
                    typeof(DataRecordPresenter), true) as DataRecordPresenter;
            if (drp == null)
                return;

            if (drp.Record != null)
            {
                frmListaTemas temas = new frmListaTemas(idProducto, pIus,true);
                temas.ShowDialog();
                //Lanzar ventana con el listado de temas a los que esta asociada la tesis
             //   btnVisualizar_Click(sender, null);
            }
        }

        private void BtnSustituir_Click(object sender, RoutedEventArgs e)
        {
            TextoS.Visibility = Visibility.Visible;
            TxtNumeroIus.Visibility = Visibility.Visible;
            BtnSustituye.Visibility = Visibility.Visible;
        }

        private void TxtNumeroIus_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            // Regex NumEx = new Regex(@"^\d+(?:.\d{0,2})?$"); 
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text 
            return !regex.IsMatch(text);
        }

        private void BtnSustituye_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNumeroIus.Text.Length < 6 || TxtNumeroIus.Text.Length > 7)
            {
                MessageBox.Show("Introduzca un número IUS Válido");
                return;
            }

            TesisModel model = new TesisModel(idProducto);
            model.SustituyeTesis(this.pIus, Convert.ToInt64(TxtNumeroIus.Text));

            TextoS.Visibility = Visibility.Hidden;
            TxtNumeroIus.Visibility = Visibility.Hidden;
            BtnSustituye.Visibility = Visibility.Hidden;

            Window_Loaded(null, null);
        }
    }
}
