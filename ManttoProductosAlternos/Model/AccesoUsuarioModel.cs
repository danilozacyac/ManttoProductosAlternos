
namespace ManttoProductosAlternos.Model
{
    public static class AccesoUsuarioModel
    {

        private static string nombre;

        public static string Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value;
            }
        }

        private static string usuario;
        public static string Usuario
        {
            get { return usuario; }
            set
            {
                usuario = value;
            }
        }

        private static string pwd;
        public static string Pwd
        {
            get { return pwd; }
            set
            {
                pwd = value;
            }
        }

        private static int llave;
        public static int Llave
        {
            get { return llave; }
            set
            {
                llave = value;
            }
        }


        private static int grupo;
        public static int Grupo
        {
            get { return grupo; }
            set
            {
                grupo = value;
            }
        }

        private static string programas;
        public static string Programas
        {
            get { return programas; }
            set
            {
                programas = value;
            }
        }
    }
}
