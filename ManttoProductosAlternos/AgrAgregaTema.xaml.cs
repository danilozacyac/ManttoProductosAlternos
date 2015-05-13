using System;
using System.Windows;
using System.Windows.Controls;
using ManttoProductosAlternos.Dto;
using ManttoProductosAlternos.Model;
using ManttoProductosAlternos.Utils;
using System.Windows.Media;
using ScjnUtilities;
using System.Collections.ObjectModel;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for AgrAgregaTema.xaml
    /// </summary>
    public partial class AgrAgregaTema : Window
    {
        private const int WinHeightAgregaTema = 180;
        private const int WinHeightAgregaTemaLargo = 452;

        //private int nivelPadre = 0;
        //private int idPadre = 0;

        /// <summary>
        /// Es el tema que se va a agregar
        /// </summary>
        private Temas temaActual;

        /// <summary>
        /// Cuando se agrega un tema nuevo el tema padre es aquel que se selecciono en la ventana previa
        /// y tendrá como hijo al nuevo tema, a menos que este sea generado como cabeza de estructura
        /// </summary>
        private Temas temaPadre;

        /// <summary>
        /// En caso de que el tema que se esta actualizando vaya a cambiar de nivel
        /// el tema seleccionado será su nuevo tema padre
        /// </summary>
        private Temas temaSeleccionado;

        private ObservableCollection<Temas> arbolTemas;

        private bool isUpdating = false;

        //private int idActNuevoPadre = 0;
        //private int nivelActNuevoPadre = 0;
        //private int idProducto = 0;

        //public AgrAgregaTema(int idPadre, int nivelPadre,int idProducto) 
        //{
        //    InitializeComponent();

        //    this.idPadre = idPadre;
        //    this.nivelPadre = nivelPadre;
        //    this.idProducto = idProducto;
        //}

        /// <summary>
        /// Agregar un tema al catálogo de temas existentes
        /// </summary>
        /// <param name="temaPadre">Tema superior de aquel que se va a ingresar</param>
        /// <param name="arbolTemas">Catálogo de temas</param>
        public AgrAgregaTema(Temas temaPadre, ObservableCollection<Temas> arbolTemas)
        {
            InitializeComponent();
            this.temaPadre = temaPadre;
            this.arbolTemas = arbolTemas;

            this.temaActual = new Temas();
            temaActual.IdProducto = temaPadre.IdProducto;
        }

        /// <summary>
        /// Actualizar uno de los temas del catálogo, la modificación puede ser de la descripción o 
        /// de la posición dentro del árbol
        /// </summary>
        /// <param name="temaPorActualizar">Tema que se va a modificar</param>
        /// <param name="arbolTemas">Catálogo de temas</param>
        /// <param name="isUpdating">Verificación de actualización</param>
        public AgrAgregaTema(Temas temaPorActualizar, ObservableCollection<Temas> arbolTemas,bool isUpdating)
        {
            InitializeComponent();
            this.temaActual = temaPorActualizar;
            this.arbolTemas = arbolTemas;
            this.isUpdating = isUpdating;

            txtTema.Text = temaActual.Tema;
            btnAgregar.Content = "Actualizar";
            this.Title = "Actualizar Tema";
        }

        //public AgrAgregaTema(int idPadre, int nivelPadre, Temas temaActualizar,int idProducto) 
        //{
        //    InitializeComponent();

            
        //    this.idPadre = idPadre;
        //    this.nivelPadre = nivelPadre;
        //    this.temaActualizar = temaActualizar;
        //    this.idProducto = idProducto;
            
        //    txtTema.Text = temaActualizar.Tema;
        //    btnAgregar.Content = "Actualizar";
        //    this.Title = "Actualizar Tema";
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tvAgraria.Items.Clear();
            if (!isUpdating)
            {
                this.Height = WinHeightAgregaTema;
                chkNodoPadre.Visibility = Visibility.Visible;
            }
            else
            {
                this.Height = WinHeightAgregaTema;
                chkCambiarPosicion.Visibility = Visibility.Visible;
                chkNodoPadre.Visibility = Visibility.Hidden;
                tvAgraria.Visibility = Visibility.Hidden;
            }

            if (temaActual.IdProducto != 1)
            {
                chkNodoPadre.Visibility = Visibility.Hidden;
                chkNodoPadre.IsChecked = true;
            }

        }

        private void BtnAgregarClick(object sender, RoutedEventArgs e)
        {
            TemasModel temasModel = new TemasModel(temaActual.IdProducto);

            if (isUpdating)  //Actualización de temas
            {
                if (chkCambiarPosicion.IsChecked == true && chkNodoPadre.IsChecked == true)
                {
                    temaActual.Nivel = 0;
                    temaActual.Padre = 0;
                }
                else if (chkCambiarPosicion.IsChecked == true && chkNodoPadre.IsChecked == false)
                {
                    if (tvAgraria.SelectedItem != null)
                    {
                        temaActual.Nivel = temaSeleccionado.Nivel + 1;
                        temaActual.Padre = temaSeleccionado.IdTema;
                    }
                    else
                    {
                        MessageBox.Show("Si la casilla de cambio de posición se encuentra activa debe seleccionar un tema",
                            "Atención : ", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                temasModel.ActualizaTema(temaActual);
            }
            else             //Tema nuevo
            {
                temaActual.Tema = (temaActual.IdProducto == 1) ? txtTema.Text.ToUpper() : txtTema.Text;
                temaActual.TemaStr = StringUtilities.PrepareToAlphabeticalOrder(txtTema.Text);
                temaActual.Orden = 0;
                temaActual.LInicial = Convert.ToChar(txtTema.Text.Substring(0, 1).ToUpper());

                if (chkNodoPadre.IsChecked == true)
                {
                    temaActual.Nivel = 0;
                    temaActual.Padre = 0;
                    arbolTemas.Add(temaActual);
                }
                else
                {
                    temaActual.Nivel = temaPadre.Nivel + 1;
                    temaActual.Padre = temaPadre.IdTema;
                    temaPadre.SubTemas.Add(temaActual);

                }
                temasModel.InsertaTemaNuevo(temaActual);

                
            }


            

            
            //tema.IdTema = VarGlobales.idSiguiente;
            //VarGlobales.temaNuevo = tema;

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
            this.Height = WinHeightAgregaTema;
        }

        private void ChkNodoPadreUnchecked(object sender, RoutedEventArgs e)
        {
            if (temaActualizar != null)
            {
                if (tvAgraria.Items.Count == 0)
                {
                    this.Height = WinHeightAgregaTemaLargo;
                    GeneraArbol gArbol = new GeneraArbol();

                    foreach (TreeViewItem tema in gArbol.GeneraAgraria(0,idProducto))
                    {
                        tvAgraria.Items.Add(tema);
                    }
                }
                else
                {
                    this.Height = WinHeightAgregaTemaLargo;
                }
            }
        }

        private void TvAgrariaSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            temaSeleccionado = tvAgraria.SelectedItem as Temas;
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
