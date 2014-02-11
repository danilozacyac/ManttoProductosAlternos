using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Editors;
using ManttoProductosAlternos.DTO;
using ManttoProductosAlternos.Interface;
using ManttoProductosAlternos.Model;

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

        private List<DocumentoTO> listaDocumentos;

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

        private void TvAgrariaSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
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

            this.SetDataInGrid();
        }

        private void SetDataInGrid()
        {
            listaDocumentos = documentos.GetDocumentosRelacionados(selectedTag);
            dgTesis.DataContext = listaDocumentos;
            txtTotal.Text = dgTesis.Records.Count + " registro(s)";
        }

        private void BtnAgregarClick(object sender, RoutedEventArgs e)
        {
            if (txtIUS.Text.Length > 0 && selectedTag != 0)
            {
                long numIus = Convert.ToInt32(txtIUS.Text);

                bool existeRelacion = false;
                foreach (DocumentoTO doc in listaDocumentos)
                {
                    if (doc.Id == numIus)
                    {
                        existeRelacion = true;
                        break;
                    }
                }

                if (!existeRelacion)
                {

                    DocumentoTO documento = documentos.GetDocumentoPorIus(numIus);

                    if (documento != null)
                        documentos.SetDocumento(documento, selectedTag);
                    else
                        MessageBox.Show("Ingresar un número de IUS válido");

                    this.SetDataInGrid();
                }
                else
                {
                    MessageBox.Show("El tema ya fue relacionado con esta ejecutoria/voto");
                }
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

        private void BtnEliminarClick(object sender, RoutedEventArgs e)
        {
            documentos.DeleteDocumento(docSeleccionado, selectedTag);
            this.SetDataInGrid();
        }

        private void BtnSalirClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DgTesisRecordActivated1(object sender, Infragistics.Windows.DataPresenter.Events.RecordActivatedEventArgs e)
        {
            if (e.Record is DataRecord)
            {
                DataRecord myRecord = (DataRecord)e.Record;
                docSeleccionado = ((DocumentoTO)myRecord.DataItem).Id;
            }
        }

        private void TxtIusPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }

        private void TxtIusLostFocus(object sender, RoutedEventArgs e)
        {
            btnAgregar.IsDefault = false;
        }

        private void TxtIusGotFocus(object sender, RoutedEventArgs e)
        {
            btnAgregar.IsDefault = true;
        }

        private static bool IsTextAllowed(string text)
        {
            // Regex NumEx = new Regex(@"^\d+(?:.\d{0,2})?$"); 
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text 
            return regex.IsMatch(text);
        }
        
    }
}
