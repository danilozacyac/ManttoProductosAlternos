using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using ManttoProductosAlternos.Model;

namespace ManttoProductosAlternos
{
    /// <summary>
    /// Interaction logic for frmAcceso.xaml
    /// </summary>
    public partial class frmAcceso : Window
    {
        private int nIntentos = 0;
        RegistryKey rkScjn;

        public frmAcceso()
        {
            InitializeComponent();
        }

        private void btnAAceptar_Click(object sender, RoutedEventArgs e)
        {


            if (txtUsuario.Text.Length == 0)
            {
                MessageBox.Show("  Falta capturar Usuario");
                return;
            }

            if (txtPwd.Password.Length == 0)
            {
                MessageBox.Show("  Falta capturar Contraseña");
                return;
            }

            AccesoModel accesoModel = new AccesoModel();

            if (nIntentos >= 3)
            {
                MessageBox.Show("   Número de intentos excedido  ");
                Close();
            }

            if (accesoModel.ObtenerUsuarioContraseña(txtUsuario.Text.ToUpper(), txtPwd.Password.ToUpper().ToString()) == false)
            {
                nIntentos++;
                MessageBox.Show("  No existe usuario y/o contraseña  ");
            }
            else
            {

                RegistryKey rkNew = Registry.CurrentUser;

                rkNew = rkNew.OpenSubKey(@"SOFTWARE\SCJN\Mantesis", true);
                rkNew.SetValue("Usuario", txtUsuario.Text, RegistryValueKind.String);



                this.Hide();
                //MainWindow mw = new MainWindow();
                //mw.ShowDialog();
                //this.Close();
            }

            if (nIntentos >= 3)
            {
                MessageBox.Show("   Número de intentos excedido  ");
                Close();
            }
        }

        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnAAceptar_Click(null, null);
            }
        }

        private void btnACancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sUsuario;
            rkScjn = Registry.CurrentUser;

            rkScjn = rkScjn.OpenSubKey(@"SOFTWARE\SCJN\Mantesis", true);

            if (rkScjn == null)
            {
                RegistryKey rKNew = Registry.CurrentUser;
                rKNew = rKNew.CreateSubKey(@"SOFTWARE\SCJN\Mantesis");
                rKNew.SetValue("Usuario", "", RegistryValueKind.String);

                rKNew.Flush();

                rkScjn = Registry.CurrentUser;
                rkScjn = rKNew.OpenSubKey(@"SOFTWARE\SCJN\Mantesis", true);
                txtUsuario.Focus();
            }
            else
            {
                sUsuario = rkScjn.GetValue("Usuario").ToString();
                txtUsuario.Text = sUsuario.ToUpper();

                txtPwd.Focus();
            }


        }
    }
}
