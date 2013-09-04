using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ManttoProductosAlternos.Model;
using ManttoProductosAlternos.DTO;
using System.Text.RegularExpressions;
using Infragistics.Windows.Editors;
using Infragistics.Windows.DataPresenter;
using ManttoProductosAlternos.Interface;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for EjeVotos.xaml
    /// </summary>
    public partial class EjeVotos : Window
    {
        private int selectedTag;
        private long docSeleccionado = 0;
        private IDocumentos documentos = new EjecutoriaModel();

        public EjeVotos()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgTesis.FieldSettings.AllowEdit = false;
            dgTesis.FieldSettings.AllowResize = false;
            Style wrapstyle = new Style(typeof(XamTextEditor));
            wrapstyle.Setters.Add(new Setter(XamTextEditor.TextWrappingProperty, TextWrapping.Wrap));
            dgTesis.FieldSettings.EditorStyle = wrapstyle;
        }

        private void tvAgraria_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            selectedTag = Convert.ToInt32(((TreeViewItem)tvAgraria.SelectedItem).Tag);

            if (selectedTag == 0)
            {
                return;
            }

            else if (selectedTag < 100)
            {
                documentos = new EjecutoriaModel();
            }
            else
            {
                selectedTag = selectedTag / 100;
                documentos = new VotosModel();
            }

            this.setDataInGrid();
        }

        private void setDataInGrid()
        {
            dgTesis.DataContext = documentos.GetDocumentosRelacionados(selectedTag);
            txtTotal.Text = dgTesis.Records.Count + " registro(s)";
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (txtIUS.Text.Length > 0 && selectedTag != 0)
            {
                long numIUS = Convert.ToInt32(txtIUS.Text);
                
                DocumentoTO documento = documentos.GetDocumentoPorIus(numIUS);

                if (documento != null)
                    documentos.SetDocumento(documento, selectedTag);
                else
                    MessageBox.Show("Ingresar un número de IUS válido");

                this.setDataInGrid();
            }
            else if (selectedTag == 0)
            {
                MessageBox.Show("No se puede realizar una asociaión con un nodo padre");
            }
            else
            {
                MessageBox.Show("Ingresar número de IUS");
            }
            txtIUS.Text = "";
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            documentos.DeleteDocumento(docSeleccionado, selectedTag);
            this.setDataInGrid();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgTesis_RecordActivated_1(object sender, Infragistics.Windows.DataPresenter.Events.RecordActivatedEventArgs e)
        {
            if (e.Record is DataRecord)
            {
                DataRecord myRecord = (DataRecord)e.Record;
                docSeleccionado = ((DocumentoTO)myRecord.DataItem).Id;
            }
        }

        private void txtIUS_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void txtIUS_LostFocus(object sender, RoutedEventArgs e)
        {
            btnAgregar.IsDefault = false;
        }

        private void txtIUS_GotFocus(object sender, RoutedEventArgs e)
        {
            btnAgregar.IsDefault = true;
        }

        private static bool IsTextAllowed(string text)
        {
            // Regex NumEx = new Regex(@"^\d+(?:.\d{0,2})?$"); 
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text 
            return !regex.IsMatch(text);
        }
        
    }
}
