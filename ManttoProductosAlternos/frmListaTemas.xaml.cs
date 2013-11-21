using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ManttoProductosAlternos.Model;
using ManttoProductosAlternos.DTO;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for frmListaTemas.xaml
    /// </summary>
    public partial class frmListaTemas : Window
    {
        private int idProducto;
        private long ius;
        private bool muestraBotonEliminarTemas;

        private List<Temas> _Temas = null;
        private Temas tema = null;

        public frmListaTemas(int idProducto, long ius, bool muestraBotonEliminarTemas)
        {
            InitializeComponent();
            this.ius = ius;
            this.idProducto = idProducto;
            this.muestraBotonEliminarTemas = muestraBotonEliminarTemas;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _Temas = new TemasModel(idProducto).GetTemasRelacionados(ius);
            lstTemas.ItemsSource = _Temas;
            lstTemas.DisplayMemberPath = "Tema";

            if (muestraBotonEliminarTemas == false)
                BtnElimRelacion.Visibility = Visibility.Hidden;
        }

        private void BtnElimRelacion_Click(object sender, RoutedEventArgs e)
        {
            
            if(tema != null)
                new TesisModel(idProducto).EliminaRelacion(ius, tema.IdTema);

            Window_Loaded(sender, e);
            /*
            temas.RemoveAt(lstTemas.SelectedIndex);
            lstTemas.ItemsSource = temas;
            */
        }

        private void lstTemas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tema = (Temas)lstTemas.SelectedItem;
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
