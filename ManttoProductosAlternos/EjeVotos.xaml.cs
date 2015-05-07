using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ManttoProductosAlternos.DTO;
using ManttoProductosAlternos.Interface;
using ManttoProductosAlternos.Model;
using ScjnUtilities;

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
            DgTesis.DataContext = listaDocumentos;
            txtTotal.Text = DgTesis.Items.Count + " registro(s)";
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

        private void TxtIusPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = StringUtilities.IsTextAllowed(e.Text);
        }

        private void TxtIusLostFocus(object sender, RoutedEventArgs e)
        {
            btnAgregar.IsDefault = false;
        }

        private void TxtIusGotFocus(object sender, RoutedEventArgs e)
        {
            btnAgregar.IsDefault = true;
        }

        private void DgTesisSelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            DocumentoTO selectedDocto = DgTesis.SelectedItem as DocumentoTO;
            docSeleccionado = selectedDocto.Id;
        }

       

    }
}
