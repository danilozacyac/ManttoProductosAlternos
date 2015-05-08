using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ManttoProductosAlternos.Model;
using ManttoProductosAlternos.Dto;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for frmPermisos.xaml
    /// </summary>
    public partial class frmPermisos : Window
    {
        Usuarios usuario = new Usuarios();

        public frmPermisos()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lstUsuarios.ItemsSource = new UsuariosModel().GetUsuarios();  //bind the data 
            lstUsuarios.SelectedValuePath = "Llave";
            lstUsuarios.DisplayMemberPath = "Nombre";
        }

        private void LstUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            usuario = (Usuarios)lstUsuarios.SelectedItem;

            String[] autorizado = usuario.Autorizados.Split(',');

            chkAgraria.IsChecked = (autorizado.Contains("1") || usuario.Grupo == 0) ? true : false;
            chkSuspension.IsChecked = (autorizado.Contains("2") || usuario.Grupo == 0) ? true : false;
            chkImprocedencia.IsChecked = (autorizado.Contains("3") || usuario.Grupo == 0) ? true : false;
            chkFacultades.IsChecked = (autorizado.Contains("4") || usuario.Grupo == 0) ? true : false;
            chkElectoral.IsChecked = (autorizado.Contains("15") || usuario.Grupo == 0) ? true : false;

            txtNombre.Text = usuario.Nombre;
            txtUsuario.Text = usuario.Usuario;
            txtPass.Password = usuario.Contrasena;

            if (usuario.Grupo < 3)
                radActivo.IsChecked = true;
            else
                radInactivo.IsChecked = true;

            groupBox1.IsEnabled = (usuario.Grupo == 0) ? false : true;
        }

        private void BtnAgregarUsuario_Click(object sender, RoutedEventArgs e)
        {
            lstUsuarios.IsEnabled = false;

            btnActualizar.Visibility = Visibility.Hidden;
            btnAceptar.Visibility = Visibility.Visible;
            btnCancelar.Visibility = Visibility.Visible;

            txtNombre.IsEnabled = true;
            txtUsuario.IsEnabled = true;

            chkAgraria.IsChecked = false;
            chkSuspension.IsChecked = false;
            chkImprocedencia.IsChecked = false;
            chkFacultades.IsChecked = false;
            chkElectoral.IsChecked = false;

            txtNombre.Text = "";
            txtUsuario.Text = "";
            txtPass.Password = "";

            radActivo.IsChecked = false;
            radInactivo.IsChecked = false;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            lstUsuarios.IsEnabled = true;

            btnActualizar.Visibility = Visibility.Visible;
            btnAceptar.Visibility = Visibility.Hidden;
            btnCancelar.Visibility = Visibility.Hidden;

            txtNombre.IsEnabled = false;
            txtUsuario.IsEnabled = false;
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (usuario.Grupo == 0)
            {
                usuario.Contrasena = txtPass.Password;
            }
            else
            {
                usuario.Contrasena = txtPass.Password;
                usuario.Grupo = (radActivo.IsChecked == true) ? 1 : 9;

                usuario.Autorizados = "";

                if (chkAgraria.IsChecked == true)
                    usuario.Autorizados = "1,";
                if (chkSuspension.IsChecked == true)
                    usuario.Autorizados = "2,";
                if (chkImprocedencia.IsChecked == true)
                    usuario.Autorizados = "3,";
                if (chkFacultades.IsChecked == true)
                    usuario.Autorizados = "4,";
                if (chkElectoral.IsChecked == true)
                    usuario.Autorizados = "15,";
            }

            UsuariosModel model = new UsuariosModel();
            model.UpdateUsuario(usuario);

            lstUsuarios.SelectedItem = usuario;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            usuario.Nombre = txtNombre.Text;
            usuario.Usuario = txtUsuario.Text;
            usuario.Contrasena = txtPass.Password;
            usuario.Grupo = (radActivo.IsChecked == true) ? 1 : 9;

            usuario.Autorizados = "";

            if (chkAgraria.IsChecked == true)
                usuario.Autorizados = "1,";
            if (chkSuspension.IsChecked == true)
                usuario.Autorizados = "2,";
            if (chkImprocedencia.IsChecked == true)
                usuario.Autorizados = "3,";
            if (chkFacultades.IsChecked == true)
                usuario.Autorizados = "4,";
            if (chkElectoral.IsChecked == true)
                usuario.Autorizados = "15,";

            UsuariosModel model = new UsuariosModel();
            model.SetNewUsuario(usuario);

            lstUsuarios.IsEnabled = true;

            btnActualizar.Visibility = Visibility.Visible;
            btnAceptar.Visibility = Visibility.Hidden;
            btnCancelar.Visibility = Visibility.Hidden;

            txtNombre.IsEnabled = false;
            txtUsuario.IsEnabled = false;

            Window_Loaded(null, null);
        }
    }
}