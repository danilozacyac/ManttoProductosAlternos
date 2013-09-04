using System.Collections.Generic;
using System.Windows;
using ManttoProductosAlternos.DTO;
using ManttoProductosAlternos.Model;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for AlertDialog.xaml
    /// </summary>
    public partial class AlertDialog : Window
    {
        private long ius;
        private int idProducto;
        private List<Temas> temas = null;

        public AlertDialog(long ius,int idProducto)
        {
            InitializeComponent();
            this.ius = ius;
            this.idProducto = idProducto;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            temas = new TemasModel(idProducto).GetTemasRelacionados(ius);

            lblTexto.Content = "La tesis que desea eliminar esta relacionada con " + temas.Count + ((temas.Count > 1) ? " \ntemas." : " \ntema.")
                                + "¿Desea continuar?";
        }

        private void btAceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnTemas_Click(object sender, RoutedEventArgs e)
        {
            frmListaTemas temas = new frmListaTemas(idProducto, ius, false);
            temas.ShowDialog();
        }
    }
}
