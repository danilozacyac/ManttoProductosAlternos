using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ManttoProductosAlternos.Model;
using ManttoProductosAlternos.DTO;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for frmPermisos.xaml
    /// </summary>
    public partial class frmPermisos : Window
    {
        Usuarios _Usuario = new Usuarios();

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

        private void lstUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _Usuario = (Usuarios)lstUsuarios.SelectedItem;

            String[] autorizado = _Usuario.Autorizados.Split(',');

            chkAgraria.IsChecked = (autorizado.Contains("1") || _Usuario.Grupo == 0) ? true : false;
            chkSuspension.IsChecked = (autorizado.Contains("2") || _Usuario.Grupo == 0) ? true : false;
            chkImprocedencia.IsChecked = (autorizado.Contains("3") || _Usuario.Grupo == 0) ? true : false;
            chkFacultades.IsChecked = (autorizado.Contains("4") || _Usuario.Grupo == 0) ? true : false;
            chkElectoral.IsChecked = (autorizado.Contains("15") || _Usuario.Grupo == 0) ? true : false;

            txtNombre.Text = _Usuario.Nombre;
            txtUsuario.Text = _Usuario.Usuario;
            txtPass.Password = _Usuario.Contrasena;

            if (_Usuario.Grupo < 3)
                radActivo.IsChecked = true;
            else
                radInactivo.IsChecked = true;

            groupBox1.IsEnabled = (_Usuario.Grupo == 0) ? false : true;

        }

        private void btnAgregarUsuario_Click(object sender, RoutedEventArgs e)
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
            if (_Usuario.Grupo == 0)
            {
                _Usuario.Contrasena = txtPass.Password;
            }
            else
            {
                _Usuario.Contrasena = txtPass.Password;
                _Usuario.Grupo = (radActivo.IsChecked == true) ? 1 : 9;

                _Usuario.Autorizados = "";

                if (chkAgraria.IsChecked == true)
                    _Usuario.Autorizados = "1,";
                if (chkSuspension.IsChecked == true)
                    _Usuario.Autorizados = "2,";
                if (chkImprocedencia.IsChecked == true)
                    _Usuario.Autorizados = "3,";
                if (chkFacultades.IsChecked == true)
                    _Usuario.Autorizados = "4,";
                if (chkElectoral.IsChecked == true)
                    _Usuario.Autorizados = "15,";
            }

            UsuariosModel model = new UsuariosModel();
            model.UpdateUsuario(_Usuario);

            lstUsuarios.SelectedItem = _Usuario;

        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            _Usuario.Nombre = txtNombre.Text;
            _Usuario.Usuario = txtUsuario.Text;
            _Usuario.Contrasena = txtPass.Password;
            _Usuario.Grupo = (radActivo.IsChecked == true) ? 1 : 9;

            _Usuario.Autorizados = "";

            if (chkAgraria.IsChecked == true)
                _Usuario.Autorizados = "1,";
            if (chkSuspension.IsChecked == true)
                _Usuario.Autorizados = "2,";
            if (chkImprocedencia.IsChecked == true)
                _Usuario.Autorizados = "3,";
            if (chkFacultades.IsChecked == true)
                _Usuario.Autorizados = "4,";
            if (chkElectoral.IsChecked == true)
                _Usuario.Autorizados = "15,";

            UsuariosModel model = new UsuariosModel();
            model.SetNewUsuario(_Usuario);

            

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
