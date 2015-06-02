using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ManttoProductosAlternos.Dto;
using ManttoProductosAlternos.Model;
using ScjnUtilities;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for FrmListaTesis.xaml
    /// </summary>
    public partial class FrmListaTesis 
    {

        private List<TesisDTO> listaDeTesisDelProducto = null;
        private ObservableCollection<Temas> temasRelacionados = null;

        private Temas selectedTema;
        private TesisDTO selectedTesis;

        private int idProducto;


        public FrmListaTesis(int idProducto)
        {
            InitializeComponent();
            this.idProducto = idProducto;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listaDeTesisDelProducto = new TesisModel(idProducto).GetTesisPorTema();
            listaDeTesisDelProducto = listaDeTesisDelProducto.Distinct().ToList();
            dgTesis.DataContext = listaDeTesisDelProducto;
            
        }

        private void BtnIusClick(object sender, RoutedEventArgs e)
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
            if (selectedTesis != null)
            {
                MessageBoxResult result = MessageBox.Show("La tesis seleccionada, que deseas eliminar, esta relacionada con " + temasRelacionados.Count + " temas. ¿Deseas continuar?",
                    "ATENCIÓN:", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    new TesisModel(idProducto).EliminaTesisLista(selectedTesis.Ius);
                    listaDeTesisDelProducto.Remove(selectedTesis);
                }
            }
        }

        private void DgTesisSelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            selectedTesis = dgTesis.SelectedItem as TesisDTO;

            temasRelacionados = new TemasModel(idProducto).GetTemasRelacionados(selectedTesis.Ius);
            LstTemas.DataContext = temasRelacionados;

            
            BtnEliminar.IsEnabled = true;
            BtnSustituir.IsEnabled = true;
        }

        public void MoveGridToIus(long nIus)
        {
            bool find = false;

            foreach (TesisDTO item in dgTesis.Items)
            {
                if (nIus == item.Ius)
                {
                    find = true;
                    dgTesis.CurrentItem = item;
                    dgTesis.SelectedItem = item;
                    dgTesis.ScrollIntoView(item);
                    break;
                }
            }

            if (!find)
                MessageBox.Show("Número de registro no encontrado");
        }

        

        private void BtnSustituir_Click(object sender, RoutedEventArgs e)
        {
            TextoS.Visibility = Visibility.Visible;
            TxtNumeroIus.Visibility = Visibility.Visible;
            BtnSustituye.Visibility = Visibility.Visible;
        }

        private void TxtNumeroIus_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = StringUtilities.IsTextAllowed(e.Text);
        }

        private void BtnSustituye_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNumeroIus.Text.Length < 6 || TxtNumeroIus.Text.Length > 7)
            {
                MessageBox.Show("Introduzca un número IUS Válido");
                return;
            }

            TesisModel model = new TesisModel(idProducto);
            model.SustituyeTesis(this.selectedTesis.Ius, Convert.ToInt64(TxtNumeroIus.Text));

            TextoS.Visibility = Visibility.Hidden;
            TxtNumeroIus.Visibility = Visibility.Hidden;
            BtnSustituye.Visibility = Visibility.Hidden;

            Window_Loaded(null, null);
        }

        private void BtnEliminaRelacion_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTema != null)
            {
                new TesisModel(idProducto).EliminaRelacion(selectedTesis.Ius, selectedTema.IdTema);

                temasRelacionados.Remove(selectedTema);
            }
            BtnEliminaRelacion.IsEnabled = false;
        }

        private void LstTemas_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedTema = LstTemas.SelectedItem as Temas;
            BtnEliminaRelacion.IsEnabled = true;

        }

        
    }
}
