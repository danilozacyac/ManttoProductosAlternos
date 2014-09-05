using System;
using System.Windows;
using System.Windows.Controls;
using ManttoProductosAlternos.DTO;
using UtilsAlternos;
using ManttoProductosAlternos.Model;
using ManttoProductosAlternos.Utils;
using System.Windows.Media;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for AgrAgregaTema.xaml
    /// </summary>
    public partial class AgrAgregaTema : Window
    {
        private int nivelPadre = 0;
        private int idPadre = 0;


        private Temas temaActualizar = null;

        private int idActNuevoPadre = 0;
        private int nivelActNuevoPadre = 0;
        private int idProducto = 0;

        public AgrAgregaTema(int idPadre, int nivelPadre,int idProducto) 
        {
            InitializeComponent();

            this.idPadre = idPadre;
            this.nivelPadre = nivelPadre;
            this.idProducto = idProducto;
        }

        public AgrAgregaTema(int idPadre, int nivelPadre, Temas temaActualizar,int idProducto) 
        {
            InitializeComponent();

            
            this.idPadre = idPadre;
            this.nivelPadre = nivelPadre;
            this.temaActualizar = temaActualizar;
            this.idProducto = idProducto;
            
            txtTema.Text = temaActualizar.Tema;
            btnAgregar.Content = "Actualizar";
            this.Title = "Actualizar Tema";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tvAgraria.Items.Clear();
            if (temaActualizar  == null)
            {
                this.Height = Constants.WinHeightAgregaTema;
                chkNodoPadre.Visibility = Visibility.Visible;
            }
            else if (temaActualizar != null)
            {
                this.Height = Constants.WinHeightAgregaTema;
                chkCambiarPosicion.Visibility = Visibility.Visible;
                chkNodoPadre.Visibility = Visibility.Hidden;
                tvAgraria.Visibility = Visibility.Hidden;
            }

            if (idProducto != 1)
            {
                chkNodoPadre.Visibility = Visibility.Hidden;
                chkNodoPadre.IsChecked = true;
            }

        }

        private void BtnAgregarClick(object sender, RoutedEventArgs e)
        {
            Temas tema = new Temas();
            tema.Tema = (idProducto == 1) ? txtTema.Text.ToUpper() : txtTema.Text;
            tema.TemaStr = MiscFunciones.GetTemasStr(txtTema.Text); 
            tema.Orden = 0;
            tema.LInicial = Convert.ToChar(txtTema.Text.Substring(0, 1).ToUpper());
            tema.IdProducto = idProducto;

            TemasModel temasModel = new TemasModel(idProducto);

            if (temaActualizar == null) //Tema Nuevo
            {
                if (chkNodoPadre.IsChecked == true)
                {
                    tema.Nivel = 0;
                    tema.Padre = 0;
                }
                else
                {
                    tema.Nivel = nivelPadre + 1;
                    tema.Padre = idPadre;
                }
                temasModel.InsertaTemaNuevo(tema);
            }
            else 
            {
                tema.IdTema = temaActualizar.IdTema;
                if (chkCambiarPosicion.IsChecked == true && chkNodoPadre.IsChecked == true)
                {
                    tema.Nivel = 0;
                    tema.Padre = 0;
                }
                else if (chkCambiarPosicion.IsChecked == true && chkNodoPadre.IsChecked == false)
                {
                    if (tvAgraria.SelectedItem != null)
                    {
                        tema.Nivel = nivelActNuevoPadre + 1;
                        tema.Padre = idActNuevoPadre;
                    }
                    else
                    {
                        MessageBox.Show("Si la casilla de cambio de posición se encuentra activa debe seleccionar un tema", "Atención : ", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                else if (chkCambiarPosicion.IsChecked == false)
                {
                    tema.Nivel = temaActualizar.Nivel;
                    tema.Padre = temaActualizar.Padre;
                }
                temasModel.ActualizaTema(tema);
            }
            tema.IdTema = VarGlobales.idSiguiente;
            VarGlobales.temaNuevo = tema;

            this.Close();
        }

        private void BtnCancelarClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChkCambiarPosicionChecked(object sender, RoutedEventArgs e)
        {
            chkNodoPadre.Visibility = Visibility.Visible;
            tvAgraria.Visibility = Visibility.Visible;
        }

        private void ChkCambiarPosicionUnchecked(object sender, RoutedEventArgs e)
        {
            chkNodoPadre.Visibility = Visibility.Collapsed;
            chkNodoPadre.IsChecked = true;
        }

        private void ChkNodoPadreChecked(object sender, RoutedEventArgs e)
        {
            this.Height = Constants.WinHeightAgregaTema;
        }

        private void ChkNodoPadreUnchecked(object sender, RoutedEventArgs e)
        {
            if (temaActualizar != null)
            {
                if (tvAgraria.Items.Count == 0)
                {
                    this.Height = Constants.WinHeightAgregaTemaLargo;
                    GeneraArbol gArbol = new GeneraArbol();

                    foreach (TreeViewItem tema in gArbol.GeneraAgraria(0,idProducto))
                    {
                        tvAgraria.Items.Add(tema);
                    }
                }
                else
                {
                    this.Height = Constants.WinHeightAgregaTemaLargo;
                }
            }
        }

        private void TvAgrariaSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem item = (TreeViewItem)tvAgraria.SelectedItem;
            idActNuevoPadre = ((Temas)item.Tag).IdTema;
            nivelActNuevoPadre = ((Temas)item.Tag).Nivel;
        }

        private void TxtTemaTextChanged(object sender, TextChangedEventArgs e)
        {
            int carRestantes = txtTema.MaxLength - txtTema.Text.Length;

            LblRestantes.Content = carRestantes;

            if (carRestantes == 0)
            {
                MessageBox.Show("Has alcanzado el límite de 250 caractéres permitidos \npara la descripción del tema");
            }

            if (carRestantes <= 10)
            {
                LblRestantes.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                LblRestantes.Foreground = new SolidColorBrush(Colors.Black);
            }

            

        }











    }
}
